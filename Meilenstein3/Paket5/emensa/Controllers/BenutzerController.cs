using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using emensa.Models;
using PasswordSecurity;
using Microsoft.AspNetCore.Http;

namespace emensa.Controllers
{
    public class BenutzerController : Controller
    {
        private readonly emensaContext _context;

        public BenutzerController(emensaContext context)
        {
            _context = context;
        }

        // GET: Benutzer/Login
        public IActionResult Login()
        {
            return View(new Benutzer());
        }

        // POST: Benutzer/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Nutzername,Password")] Benutzer benutzer)
        {
            var dbBenutzer = _context.Benutzer.Where(b => b.Nutzername.Equals(benutzer.Nutzername));
                
                                            
            if(dbBenutzer.Count() != 0){                                
                
                if(dbBenutzer.First().Aktiv == 0){
                    ViewData["LoginMessage"] = "Account nicht aktiviert!";
                    return View();  
                }

                if(dbBenutzer.First().verifyPassword(benutzer.Password)){
                    // Zugangsdaten sind korrekt
                    HttpContext.Session.SetString("user", dbBenutzer.First().Nutzername);
                    HttpContext.Session.SetString("role", dbBenutzer.First().getRole(_context.Database.GetDbConnection().ConnectionString));
                    return RedirectToAction(nameof(LoggedIn));
                }
                ViewData["LoginMessage"] = "Die eingegeben Logindaten wurden nicht gefunden!";
                ViewData["PasswordError"] = "Password falsch";
                
            }
            else{
                ViewData["LoginMessage"] = "Die eingegeben Logindaten wurden nicht gefunden!";
                ViewData["NutzernameError"] = "Nutzername falsch";
            }

            
            return View();
        }

        // GET: Benutzer/LoggedIn
        public IActionResult LoggedIn()
        {
            return View();
        }

        // POST: Benutzer/LoggedIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoggedOut()
        {

            HttpContext.Session.SetString("user", "");
            HttpContext.Session.SetString("role", "");
            ViewData["LoginMessage"] = "Sie wurden erfolgreich ausgeloggt!";
            
            return RedirectToAction(nameof(Login));
        }

        public static string printCurrentRole(ISession s){
            if(s.GetString("role") != "" && s.GetString("role") != null){
                return s.GetString("role");
            }
            return "Gast";
        }


        // GET: Benutzer/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Benutzer/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Nutzername,EMail,Vorname,Nachname,Geburtsdatum,PasswordRepeat,Password")] Benutzer benutzer, string subnutzer)
        {
            if (ModelState.IsValid)
            {
                benutzer.createHashSalt();                
                

                switch(subnutzer){
                    case "Gast":
                        Gäste g = new Gäste();
                        g.FkBenutzerNavigation = benutzer;
                        g.Grund = "";
                        g.Ablaufdatum = DateTime.Now.AddYears(1);
                        _context.Add(benutzer);
                        _context.Add(g);
                    break;
                    case "Mitarbeiter":
                        FhAngehörige fhm = new FhAngehörige();
                        fhm.FkBenutzerNavigation = benutzer;
                        Mitarbeiter m = new Mitarbeiter();
                        m.Büro = 1200;
                        m.Telefon = 112;
                        m.FkFhangeNavigation = fhm;
                        _context.Add(benutzer);
                        _context.Add(fhm);
                        _context.Add(m);
                    break;
                    case "Student":
                        FhAngehörige fhs = new FhAngehörige();
                        fhs.FkBenutzerNavigation = benutzer;
                        Student s = new Student();
                        s.Matrikelnummer = 3242322;
                        s.Studiengang = "ET";
                        s.FkFhangeNavigation = fhs;
                        _context.Add(benutzer);
                        _context.Add(fhs);
                        _context.Add(s);
                    break;
                }

                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(benutzer);
        }

#region Scafflod
        // GET: Benutzer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Benutzer.ToListAsync());
        }

        // GET: Benutzer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benutzer = await _context.Benutzer
                .FirstOrDefaultAsync(m => m.Nummer == id);
            if (benutzer == null)
            {
                return NotFound();
            }

            return View(benutzer);
        }

        // GET: Benutzer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Benutzer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nummer,Nutzername,EMail,Aktiv,Vorname,Nachname,Geburtsdatum,Alter,PasswordRepeat,Password")] Benutzer benutzer)
        {
            if (ModelState.IsValid)
            {
                benutzer.createHashSalt();                
                _context.Add(benutzer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(benutzer);
        }

        // GET: Benutzer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benutzer = await _context.Benutzer.FindAsync(id);
            if (benutzer == null)
            {
                return NotFound();
            }
            return View(benutzer);
        }

        // POST: Benutzer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nummer,Nutzername,EMail,Aktiv,Vorname,Nachname,Geburtsdatum,Alter,PasswordRepeat,Password")] Benutzer benutzer)
        {
            if (id != benutzer.Nummer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                benutzer.createHashSalt();
                try
                {
                    _context.Update(benutzer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenutzerExists(benutzer.Nummer))
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
            return View(benutzer);
        }

        // GET: Benutzer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benutzer = await _context.Benutzer
                .FirstOrDefaultAsync(m => m.Nummer == id);
            if (benutzer == null)
            {
                return NotFound();
            }

            return View(benutzer);
        }

        // POST: Benutzer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var benutzer = await _context.Benutzer.FindAsync(id);
            _context.Benutzer.Remove(benutzer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BenutzerExists(int id)
        {
            return _context.Benutzer.Any(e => e.Nummer == id);
        }

#endregion

    }
}
