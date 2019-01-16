using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Bestellungen
    {
        public Bestellungen()
        {
            BestellungEnthältMahlzeit = new HashSet<BestellungEnthältMahlzeit>();
        }

        [Key]
        public int Nummer { get; set; }
        public int BenutzerNummer { get; set; }
        public DateTime BestellZeitpunkt { get; set; }
        public DateTime Abholzeitpunkt { get; set; }
        public float? Endpreis { get; set; }

        public virtual Benutzer BenutzerNummerNavigation { get; set; }
        public virtual ICollection<BestellungEnthältMahlzeit> BestellungEnthältMahlzeit { get; set; }
    }
}
