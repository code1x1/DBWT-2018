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
            CookieWrapper cw = new CookieWrapper(Request,Response,ViewData);
            var mahlzeiten = 
                _context.Mahlzeiten
                .Join(_context.Preise,
                    mahlzeit => mahlzeit.Id,
                    preis => preis.FkMahlzeiten,
                    (mahlzeit, preis) => new MahlzeitenPreise { Mahlzeiten = mahlzeit, Preise = preis, Anzahl = ((Dictionary<string,int>)cw.getMahlzeiten())[mahlzeit.Id.ToString()] })
                .Where(mp => cw.getMahlzeiten().Keys.Contains(mp.Mahlzeiten.Id.ToString()));
            
            return View(mahlzeiten);
        }

        // POST: Bestellungen/Warenkorb
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Warenkorb(Tuple<int,int>[] array)
        {
            _cookie = new CookieWrapper(Request,Response,ViewData);
            _cookie.modCookie(array);
            var mahlzeiten = 
                _context.Mahlzeiten
                .Join(_context.Preise,
                    mahlzeit => mahlzeit.Id,
                    preis => preis.FkMahlzeiten,
                    (mahlzeit, preis) => new MahlzeitenPreise { Mahlzeiten = mahlzeit, Preise = preis, Anzahl = ((Dictionary<string,int>)_cookie.getMahlzeiten())[mahlzeit.Id.ToString()] })
                .Where(mp => _cookie.getMahlzeiten().Keys.Contains(mp.Mahlzeiten.Id.ToString()));

            return View(mahlzeiten);
        } 


        // GET: Bestellungen/DeleteWarenkorb
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public IActionResult DeleteWarenkorb()
        {
            _cookie = new CookieWrapper(Request,Response,ViewData);
            _cookie.clearAll();
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
