using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class MahlzeitenZutaten
    {
        public int Idzutaten { get; set; }
        public int Idmahlzeiten { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Mahlzeiten IdmahlzeitenNavigation { get; set; }
        public virtual Zutaten IdzutatenNavigation { get; set; }
    }
}
