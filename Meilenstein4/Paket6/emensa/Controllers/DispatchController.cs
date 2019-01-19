using System;
using System.Linq;
using System.Collections.Generic;
using emensa.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using emensa.ViewModels;

namespace emensa.Controllers
{

    public class DispatchController : Controller {

        
        public DispatchController(emensaContext context)
        {
            _context = context;
        }

        private readonly emensaContext _context;

 
        public JsonResult Bestellungen()
        {
           
            var header = Request.Headers["X-Authorize"].ToString();
            if (header != null && header.Contains("supergeheim")) {
                
                var _c = _context; 

                var bestellungenListe = 
                    from _bestellung in _c.Bestellungen
                    where _bestellung.Abholzeitpunkt > DateTime.Now.AddMinutes(30)
                    group _bestellung by _bestellung.Nummer into bestellung
                    let b = new { frist = bestellung.First() }
                    let bem = _c.BestellungEnthältMahlzeit.Where(x=> x.FkBestellungen == b.frist.Nummer)
                    let Mahlzeiten = bem.Join(_c.Mahlzeiten, (bems => bems.FkMahlzeit), (mz => mz.Id),(bems, mz) 
                                                    => new { Mahlzeit = new { Anzahl=bems.Anzahl, Name=mz.Name, Vorrat= mz.Vorrat, Kategorie=mz.FkKategorieNavigation.Bezeichnung} }) 
                    let benutzer = new {b.frist.BenutzerNummerNavigation.Vorname,b.frist.BenutzerNummerNavigation.Nachname,b.frist.BenutzerNummerNavigation.Nutzername,b.frist.BenutzerNummerNavigation.EMail}
                    select new { Benutzer=benutzer, Abholung=b.frist.Abholzeitpunkt, Bestellnummer=b.frist.Nummer, Mahlzeiten };

                return Json(bestellungenListe);
                
            }
            Response.StatusCode = 404;
            return Json("{ 'message' : 'Sie sind nicht befugt auf die Daten zuzugreifen', 'status' : '" + Response.StatusCode + "' }");
        }
    }
}