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
using MySql.Data.MySqlClient;
using System.Diagnostics;

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
                ViewData["LoginName"] = benutzer.Nutzername;
                
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
        public IActionResult Register([Bind("Nutzername,EMail,Vorname,Nachname,Geburtsdatum,PasswordRepeat,Password")] Benutzer benutzer, 
                                                    string subnutzer, string grund, int buro, int tele, int matrikelnummer, string studiengang)
        {

                benutzer.createHashSalt();
                // innerhalb der Connection con eine Transaktion beginnen
                MySqlConnection con = new MySqlConnection(Startup.ConnectionString);
                try{
                    con.Open();
                    MySqlTransaction tr = con.BeginTransaction();                
                    try{       
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = con;
                            cmd.Transaction = tr;
                            cmd.CommandText = $@"INSERT INTO `Benutzer` 
                            (`Vorname`, `Nachname`, `E-Mail`, `Nutzername`, 
                                    `Anlegedatum`, `Geburtsdatum`, `Salt`, `Hash`, `Aktiv`) 
                            VALUES ('{benutzer.Vorname}', '{benutzer.Nachname}', '{benutzer.EMail}', '{benutzer.Nutzername}',
                                    '{DateTime.Now.ToShortDateString()}', '{benutzer.Geburtsdatum}', '{benutzer.SaltString}', '{benutzer.HashString}', 0);";
                            int rows = cmd.ExecuteNonQuery(); // DML
                            
                            
                            switch(subnutzer){
                                case "Gast":
                                    cmd.CommandText = $@"
                                    INSERT INTO `Gäste` (`Grund`, Ablaufdatum, `fkBenutzer`) VALUES 
                                    ('{grund}', '{DateTime.Now.AddYears(1).ToShortDateString()}', {cmd.LastInsertedId})";
                                    rows = cmd.ExecuteNonQuery();
                                break;
                                case "Mitarbeiter":
                                    cmd.CommandText = $@"
                                    INSERT INTO `FH Angehörige` (`fkBenutzer`) 
                                    VALUES ({cmd.LastInsertedId})";
                                    rows = cmd.ExecuteNonQuery();

                                    if(rows>0){
                                        cmd.CommandText = $@"
                                        INSERT INTO `Mitarbeiter` (`Büro`, `Telefon`, `fkFHange`) 
                                        VALUES ({buro}, {tele}, {cmd.LastInsertedId})";
                                        rows = cmd.ExecuteNonQuery();
                                    }

                                break;
                                case "Student":
                                    cmd.CommandText = $@"
                                    INSERT INTO `FH Angehörige` (`fkBenutzer`) 
                                    VALUES ({cmd.LastInsertedId})";
                                    rows = cmd.ExecuteNonQuery();

                                    if(rows>0){
                                        cmd.CommandText = $@"
                                        INSERT INTO `Student` (`Studiengang`, `Matrikelnummer`, `fkFHange`) 
                                        VALUES ('{studiengang}', {matrikelnummer}, {cmd.LastInsertedId})";
                                        rows = cmd.ExecuteNonQuery();
                                    }
                                break;
                            }         


                            // alle fehlerfrei?  commit!
                            tr.Commit();
                    } catch(Exception e){
                        Debug.Print(e.StackTrace);
                        // falls es Probleme gab
                        tr.Rollback();
                        return View(benutzer);

                    } finally{
                        
                    }
        
                
                }catch(Exception e){
                    Debug.Print(e.StackTrace);
                } finally{
                    con.Close();    
                }

            return RedirectToAction(nameof(Login));
        
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
