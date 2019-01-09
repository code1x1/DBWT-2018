using System.ComponentModel.DataAnnotations;
using emensa.Models;
using Microsoft.AspNetCore.Http;

namespace emensa.ViewModels {
    public class MahlzeitenPreise
    {
        public Mahlzeiten Mahlzeiten { get; internal set; }
        public Preise Preise { get; internal set; }
    }
}