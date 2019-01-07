using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Mahlzeiten
    {
        public Mahlzeiten()
        {
            BestellungEnthältMahlzeit = new HashSet<BestellungEnthältMahlzeit>();
            Kommentare = new HashSet<Kommentare>();
            MahlzeitDeklarationen = new HashSet<MahlzeitDeklarationen>();
            MahlzeitenBilder = new HashSet<MahlzeitenBilder>();
            MahlzeitenZutaten = new HashSet<MahlzeitenZutaten>();
            Preise = new HashSet<Preise>();
        }
        [Key]
        public int Id { get; set; }
        public string Beschreibung { get; set; }
        public int Vorrat { get; set; }
        public int? FkKategorie { get; set; }
        public byte? Verfügbar { get; set; }
        public string Name { get; set; }

        public virtual Kategorien FkKategorieNavigation { get; set; }
        public virtual ICollection<BestellungEnthältMahlzeit> BestellungEnthältMahlzeit { get; set; }

        public virtual ICollection<Kommentare> Kommentare { get; set; }
        public virtual ICollection<MahlzeitDeklarationen> MahlzeitDeklarationen { get; set; }
        public virtual ICollection<MahlzeitenBilder> MahlzeitenBilder { get; set; }
        public virtual ICollection<MahlzeitenZutaten> MahlzeitenZutaten { get; set; }
        public virtual ICollection<Preise> Preise { get; set; }
    }
}
