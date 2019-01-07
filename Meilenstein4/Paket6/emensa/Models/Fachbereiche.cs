using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Fachbereiche
    {
        public Fachbereiche()
        {
            GehörtZuFachbereiche = new HashSet<GehörtZuFachbereiche>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Adresse { get; set; }

        public virtual ICollection<GehörtZuFachbereiche> GehörtZuFachbereiche { get; set; }
    }
}
