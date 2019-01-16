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

                var bestellungenListe = from _benutzer in _c.Benutzer
                    join _bestellung in _c.Bestellungen on  _benutzer.Nummer equals _bestellung.BenutzerNummer
                    join _bestellungenthaltmahlzeit in _c.BestellungEnthältMahlzeit on _bestellung.Nummer equals _bestellungenthaltmahlzeit.FkBestellungen
                    join _mahlzeit in _c.Mahlzeiten on _bestellungenthaltmahlzeit.FkMahlzeit equals _mahlzeit.Id
                    join _kategorie in _c.Kategorien on _mahlzeit.FkKategorie equals _kategorie.Id
                    group _bestellung by _bestellung.Nummer into bestellung
                    let b = new { frist = bestellung.First() }
                    let m = new { mahlzeiten=b.frist.BestellungEnthältMahlzeit }
                    let benutzer = new {b.frist.BenutzerNummerNavigation.Vorname,b.frist.BenutzerNummerNavigation.Nachname,b.frist.BenutzerNummerNavigation.Nutzername,b.frist.BenutzerNummerNavigation.EMail}
                    select new{ benutzer, b.frist.Abholzeitpunkt, b.frist.BestellZeitpunkt, m.mahlzeiten };
                    /*let benutzer = new { Vorname=_benutzer.Vorname,Nachname=_benutzer.Nachname,Nutzername=_benutzer.Nutzername,EMail=_benutzer.EMail }
                    let mahlzeiten = new { _mahlzeit.Name, _mahlzeit.MahlzeitenZutaten, _mahlzeit.FkKategorieNavigation}
                    select new {benutzer,Abholung=_bestellung.Abholzeitpunkt,Bestellnummer=_bestellung.Nummer, mahlzeiten};
                    "Vorname": "Bugs",
                    "Nachname": "Findmore",
                    "Nutzername": "bugfin",
                    "EMail": "dbwt2018@ismypassword.com"
                    
                    */
                return Json(bestellungenListe);
                
            }
            Response.StatusCode = 404;
            return Json("{ 'message' : 'Sie sind nicht befugt auf die Daten zuzugreifen', 'status' : '" + Response.StatusCode + "' }");
        }
    }
}