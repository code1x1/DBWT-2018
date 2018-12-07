using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class BestellungEnthältMahlzeit
    {
        public int Anzahl { get; set; }
        public int FkMahlzeit { get; set; }
        public int FkBestellungen { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Bestellungen FkBestellungenNavigation { get; set; }
        public virtual Mahlzeiten FkMahlzeitNavigation { get; set; }
    }
}
