using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using emensa.Models;
using Microsoft.AspNetCore.Http;
using emensa.Extension;
using Newtonsoft.Json;

namespace emensa.Controllers
{
    public class MahlzeitenController : Controller
    {
        //https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
        private readonly emensaContext _context;
        private CookieWrapper _cookie;

        public MahlzeitenController(emensaContext context)
        {
            _context = context;
        }

        // GET: Mahlzeiten
        public async Task<IActionResult> Liste(int? KategorieList, bool? verfugbar, bool? vegetar, bool? vegan, bool? all)
        {
            ViewData["KategorieListe"] = _context.Kategorien.ToList();
            if(KategorieList != null){
                ViewData["KategorieSelected"] = _context.Kategorien.Where(x => x.Id == KategorieList).First();
            }
            IQueryable<emensa.Models.Mahlzeiten> mahlzeiten;
            
            mahlzeiten = _context.Mahlzeiten.Include(m => m.FkKategorieNavigation);
            
            if(true == all){
                return View(await mahlzeiten.Take(8).ToListAsync());
            }

            if(KategorieList!= null && KategorieList!= 0)
                mahlzeiten = mahlzeiten.Where(x => x.FkKategorieNavigation.Id == KategorieList);

            if(true == verfugbar)
                mahlzeiten = mahlzeiten.Where(x => x.Vorrat > 0);

            if(true == vegetar)
                mahlzeiten = mahlzeiten.Include(m => m.MahlzeitenZutaten).Where(x => x.MahlzeitenZutaten.Any(y => y.IdzutatenNavigation.Vegetarisch != Convert.ToByte(vegetar)));

            if(true == vegan)
                mahlzeiten = mahlzeiten.Include(m => m.MahlzeitenZutaten).Where(x => x.MahlzeitenZutaten.Any(y => y.IdzutatenNavigation.Vegan != Convert.ToByte(vegan)));

            return View(await mahlzeiten.Take(8).ToListAsync());
        }

        // POST: Mahlzeiten/Details/5
        [HttpPost]
        public async Task<IActionResult> Details([Bind("Id,Name")] Mahlzeiten mz)
        {
            int? id = mz.Id;
            
            if (id == null)
            {
                return NotFound();
            }

            var mahlzeiten = await _context.Mahlzeiten
                .Include(m => m.FkKategorieNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mahlzeiten == null)
            {
                return NotFound();
            }
            ViewData["vorbestellt"] = $"{mahlzeiten.Name} zur Bestellung hinzugefügt";
            _cookie = new CookieWrapper(Request,Response,ViewData,HttpContext.Session);
            _cookie.addMahlzeit(mahlzeiten);
            

            return View(mahlzeiten);
        }

        public static int getImageID(int mahlzeitid){
            
            int imageId = (int)MySqlWrapper.exexuteScalar<int>($@"SELECT b.ID FROM Bilder as b
                                    JOIN MahlzeitenBilder as mb ON mb.IDBilder=b.ID
                                    JOIN Mahlzeiten as m ON mb.IDMahlzeiten=m.ID
                                    WHERE m.ID={mahlzeitid}");

            return imageId;
        }

        public static List<string> zutatenListe(int id){
            return MySqlWrapper.exexuteColumn($@"SELECT z.`Name` FROM MahlzeitenZutaten AS mz 
                                JOIN Zutaten AS z ON z.ID=mz.IDZutaten 
                                WHERE mz.IDMahlzeiten={id}","Name");
        }

        public static string getImageAltText(int mahlzeitid){
            string imageId = (string)MySqlWrapper.exexuteScalar<string>($@"SELECT b.`Alt-Text` FROM Bilder as b
                                    JOIN MahlzeitenBilder as mb ON mb.IDBilder=b.ID
                                    JOIN Mahlzeiten as m ON mb.IDMahlzeiten=m.ID
                                    WHERE m.ID={mahlzeitid}");

            return imageId;
        }

        public static string getImageCopyright(int mahlzeitid){
            string imageId = MySqlWrapper.exexuteScalar<string>($@"SELECT b.Copyright FROM Bilder as b
                                    JOIN MahlzeitenBilder as mb ON mb.IDBilder=b.ID
                                    JOIN Mahlzeiten as m ON mb.IDMahlzeiten=m.ID
                                    WHERE m.ID={mahlzeitid}");

            return imageId;
        }

        public static string getPreis(int mahlzeitid, ISession s){
            string preis = "0"; 
            
            if(s.GetString("role") != "" && s.GetString("role") != null){
                if(s.GetString("role") == "Student"){
                    preis = MySqlWrapper.exexuteReaderPreis($@"SELECT p.Studentpreis
                                                                from Preise p 
                                                                WHERE p.fkMahlzeiten={mahlzeitid}","Studentpreis");
                
                } else{
                    preis = MySqlWrapper.exexuteReaderPreis($@"SELECT p.`MA-Preis`
                                                                from Preise p 
                                                                WHERE p.fkMahlzeiten={mahlzeitid}","MA-Preis");

                }
                return preis;           
            }
            preis = MySqlWrapper.exexuteReaderPreis($@"SELECT p.Gastpreis 
                                                                from Preise p 
                                                                WHERE p.fkMahlzeiten={mahlzeitid}","Gastpreis");
            return preis;
        }

#region Scaffold
        // GET: Mahlzeiten
        public async Task<IActionResult> Index()
        {
            var emensaContext = _context.Mahlzeiten.Include(m => m.FkKategorieNavigation);
            return View(await emensaContext.ToListAsync());
        }

        // GET: Mahlzeiten/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var mahlzeiten = await _context.Mahlzeiten
                .Include(m => m.FkKategorieNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mahlzeiten == null)
            {
                return NotFound();
            }

            return View(mahlzeiten);
        }

        // GET: Mahlzeiten/Create
        public IActionResult Create()
        {
            ViewData["FkKategorie"] = new SelectList(_context.Kategorien, "Id", "Bezeichnung");
            return View();
        }

        // POST: Mahlzeiten/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Beschreibung,Vorrat,FkKategorie,Verfügbar,Name")] Mahlzeiten mahlzeiten)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mahlzeiten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkKategorie"] = new SelectList(_context.Kategorien, "Id", "Bezeichnung", mahlzeiten.FkKategorie);
            return View(mahlzeiten);
        }

        // GET: Mahlzeiten/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mahlzeiten = await _context.Mahlzeiten.FindAsync(id);
            if (mahlzeiten == null)
            {
                return NotFound();
            }
            ViewData["FkKategorie"] = new SelectList(_context.Kategorien, "Id", "Bezeichnung", mahlzeiten.FkKategorie);
            return View(mahlzeiten);
        }

        // POST: Mahlzeiten/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Beschreibung,Vorrat,FkKategorie,Verfügbar,Name")] Mahlzeiten mahlzeiten)
        {
            if (id != mahlzeiten.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mahlzeiten);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MahlzeitenExists(mahlzeiten.Id))
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
            ViewData["FkKategorie"] = new SelectList(_context.Kategorien, "Id", "Bezeichnung", mahlzeiten.FkKategorie);
            return View(mahlzeiten);
        }

        // GET: Mahlzeiten/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mahlzeiten = await _context.Mahlzeiten
                .Include(m => m.FkKategorieNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mahlzeiten == null)
            {
                return NotFound();
            }

            return View(mahlzeiten);
        }

        // POST: Mahlzeiten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mahlzeiten = await _context.Mahlzeiten.FindAsync(id);
            _context.Mahlzeiten.Remove(mahlzeiten);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MahlzeitenExists(int id)
        {
            return _context.Mahlzeiten.Any(e => e.Id == id);
        }
#endregion
    }
}
