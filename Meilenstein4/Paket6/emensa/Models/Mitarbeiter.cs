using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Mitarbeiter
    {
        public int Büro { get; set; }
        public int Telefon { get; set; }
        public int FkFhange { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual FhAngehörige FkFhangeNavigation { get; set; }
    }
}
