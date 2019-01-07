using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Kategorien
    {
        public Kategorien()
        {
            InverseFkOberKategorieNavigation = new HashSet<Kategorien>();
            Mahlzeiten = new HashSet<Mahlzeiten>();
        }
        [Key]
        public int Id { get; set; }
        public string Bezeichnung { get; set; }
        public int? FkOberKategorie { get; set; }
        public int? FkBild { get; set; }

        public virtual Bilder FkBildNavigation { get; set; }
        public virtual Kategorien FkOberKategorieNavigation { get; set; }
        public virtual ICollection<Kategorien> InverseFkOberKategorieNavigation { get; set; }
        public virtual ICollection<Mahlzeiten> Mahlzeiten { get; set; }

    }
}
