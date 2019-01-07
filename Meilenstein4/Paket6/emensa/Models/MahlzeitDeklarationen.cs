using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class MahlzeitDeklarationen
    {
        public string FkDeklaration { get; set; }
        public int FkMahlzeit { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Deklarationen FkDeklarationNavigation { get; set; }
        public virtual Mahlzeiten FkMahlzeitNavigation { get; set; }
    }
}
