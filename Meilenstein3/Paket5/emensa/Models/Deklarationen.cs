using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Deklarationen
    {
        public Deklarationen()
        {
            MahlzeitDeklarationen = new HashSet<MahlzeitDeklarationen>();
        }
        
        public string Zeichen { get; set; }
        public string Beschriftung { get; set; }

        public virtual ICollection<MahlzeitDeklarationen> MahlzeitDeklarationen { get; set; }
    }
}
