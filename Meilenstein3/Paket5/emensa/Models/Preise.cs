using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Preise
    {
        public int Jahr { get; set; }
        public float Gastpreis { get; set; }
        public float Studentpreis { get; set; }
        public float MaPreis { get; set; }
        public int FkMahlzeiten { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Mahlzeiten FkMahlzeitenNavigation { get; set; }
    }
}
