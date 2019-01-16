using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using emensa.Models;
using Microsoft.AspNetCore.Http;

namespace emensa.ViewModels {
    public class JsonBestellung
    {
        public Benutzer benutzers;
        public DateTime Abholzeit;
        public Int32 Bestellnummer;
        public List<Mahlzeiten> mahlzeitens;

    }
}