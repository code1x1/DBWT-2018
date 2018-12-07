using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Bilder
    {
        public Bilder()
        {
            Kategorien = new HashSet<Kategorien>();
            MahlzeitenBilder = new HashSet<MahlzeitenBilder>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string AltText { get; set; }
        [Required]
        public string Titel { get; set; }
        [Required]
        public byte[] Binärdaten { get; set; }
        [Required]
        public string Copyright { get; set; }

        public virtual ICollection<Kategorien> Kategorien { get; set; }
        public virtual ICollection<MahlzeitenBilder> MahlzeitenBilder { get; set; }
    }
}
