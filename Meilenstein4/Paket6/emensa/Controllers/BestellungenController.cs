using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using emensa.Models;
using emensa.ViewModels;
using emensa.Extension;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace emensa.Controllers
{
    public class BestellungenController : Controller
    {
        private readonly emensaContext _context;
        private CookieWrapper _cookie;

        public BestellungenController(emensaContext context)
        {
            _context = context;
        }


        // GET: Bestellungen/Warenkorb
        public IActionResult Warenkorb()
        {
            _cookie = new CookieWrapper(Request,Response,ViewData,HttpContext.Session);
            var mahlzeiten = 
                _context.Mahlzeiten
                .Join(_context.Preise,
                    mahlzeit => mahlzeit.Id,
                    preis => preis.FkMahlzeiten,
                    (mahlzeit, preis) => new MahlzeitenPreise { Mahlzeiten = mahlzeit, Preise = preis, Anzahl = ((Dictionary<string,int>)_cookie.getMahlzeiten())[mahlzeit.Id.ToString()] })
                .Where(mp => _cookie.getMahlzeiten().Keys.Contains(mp.Mahlzeiten.Id.ToString()));
            
            return View(mahlzeiten);
        }

        // POST: Bestellungen/Warenkorb
        // Update der Warenkorb Anzahl
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Warenkorb(int[] arrayID, int[] arrayAnzahl)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for(int i=0;i<arrayID.Length;i++)
            {
                if(arrayAnzahl[i] != 0){
                dict.Add(Convert.ToString(arrayID[i]), arrayAnzahl[i]);
                }
                
            }
            _cookie = new CookieWrapper(Request,Response,ViewData,HttpContext.Session);
            _cookie.modCookie(dict);
            return RedirectToAction(nameof(Warenkorb));
        } 


        // GET: Bestellungen/DeleteWarenkorb
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public IActionResult DeleteWarenkorb()
        {
            _cookie = new CookieWrapper(Request,Response,ViewData,HttpContext.Session);
            _cookie.clearAll();
             return RedirectToAction(nameof(Warenkorb));
        }


        // POST: Bestellungen/SubmitWarenkorb
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitWarenkorb(DateTime abholzeit)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
            {
                return Redirect("/Benutzer/Login");
            }

            _cookie = new CookieWrapper(Request, Response, ViewData, HttpContext.Session);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var endpreisQuery = _context.Mahlzeiten
                .Where(mp => _cookie.getMahlzeiten().Keys.Contains(mp.Id.ToString()))
                .Join(_context.Preise,
                    mahlzeit => mahlzeit.Id,
                    preis => preis.FkMahlzeiten,
                    (mahlzeit, preis) => new { preis.Gastpreis, preis.MaPreis, preis.Studentpreis, mahlzeit.Id });

                float endpreis = endpreisQuery.Sum(x => x.Gastpreis * _cookie.getMahlzeiten()[x.Id.ToString()]);

                if (HttpContext.Session.GetString("role") == "Student")
                {
                    endpreis = endpreisQuery.Sum(x => x.Studentpreis * _cookie.getMahlzeiten()[x.Id.ToString()]);
                }
                else if (HttpContext.Session.GetString("role") == "Mitarbeiter")
                {
                    endpreis = endpreisQuery.Sum(x => x.MaPreis * _cookie.getMahlzeiten()[x.Id.ToString()]);
                }

                try
                {
                    Bestellungen bestellungen = new Bestellungen();
                    bestellungen.BenutzerNummer = _context.Benutzer.Where(x => x.Nutzername == HttpContext.Session.GetString("user")).First().Nummer;
                    bestellungen.Abholzeitpunkt = abholzeit;
                    bestellungen.BestellZeitpunkt = DateTime.Now;
                    bestellungen.Endpreis = endpreis;
                    _context.Add(bestellungen);

                    foreach (var item in _cookie.getMahlzeiten())
                    {
                        BestellungEnthältMahlzeit bestellungMahlzeit = new BestellungEnthältMahlzeit();
                        bestellungMahlzeit.Anzahl = item.Value;
                        bestellungMahlzeit.FkBestellungen = bestellungen.Nummer;
                        bestellungMahlzeit.FkMahlzeit = Convert.ToInt32(item.Key);
                        _context.Add(bestellungMahlzeit);

                    }
                    // Commit transaction if all commands succeed, transaction will auto-rollback
                    // when disposed if either commands fails
                    transaction.Commit();
                    TempData["SuccessSubmit"] = "Die Bestellung wurde kostenpflichtig verbucht!";
                    _cookie.clearAll();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    transaction.Rollback();
                    // TODO: Handle failure
                    TempData["SuccessSubmit"] = "Die Bestellung konnte nicht ausgeführt werden!";
                }
            }

            try{
                _context.SaveChanges();
            }
            catch (MySqlException myex){
                Debug.WriteLine(myex);
                TempData["SuccessSubmit"] = "Die Bestellung übersteigt den Vorrat!";
            }
            catch(DbUpdateException dbex){
                Debug.WriteLine(dbex);
                TempData["SuccessSubmit"] = "Die Bestellung übersteigt den Vorrat!";
            }

            return RedirectToAction(nameof(Warenkorb));
        }

        #region Scaffolding

        // GET: Bestellungen
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bestellungen.ToListAsync());
        }

        // GET: Bestellungen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellungen = await _context.Bestellungen
                .FirstOrDefaultAsync(m => m.Nummer == id);
            if (bestellungen == null)
            {
                return NotFound();
            }

            return View(bestellungen);
        }

        // GET: Bestellungen/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bestellungen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nummer,BestellZeitpunkt,Abholzeitpunkt,Endpreis")] Bestellungen bestellungen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bestellungen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bestellungen);
        }



        // GET: Bestellungen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellungen = await _context.Bestellungen.FindAsync(id);
            if (bestellungen == null)
            {
                return NotFound();
            }
            return View(bestellungen);
        }

        // POST: Bestellungen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nummer,BestellZeitpunkt,Abholzeitpunkt,Endpreis")] Bestellungen bestellungen)
        {
            if (id != bestellungen.Nummer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestellungen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellungenExists(bestellungen.Nummer))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bestellungen);
        }

        // GET: Bestellungen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellungen = await _context.Bestellungen
                .FirstOrDefaultAsync(m => m.Nummer == id);
            if (bestellungen == null)
            {
                return NotFound();
            }

            return View(bestellungen);
        }

        // POST: Bestellungen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestellungen = await _context.Bestellungen.FindAsync(id);
            _context.Bestellungen.Remove(bestellungen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellungenExists(int id)
        {
            return _context.Bestellungen.Any(e => e.Nummer == id);
        }
    #endregion

    }

}
