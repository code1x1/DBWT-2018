using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class MahlzeitenBilder
    {
        public int Idbilder { get; set; }
        public int Idmahlzeiten { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Bilder IdbilderNavigation { get; set; }
        public virtual Mahlzeiten IdmahlzeitenNavigation { get; set; }
    }
}
