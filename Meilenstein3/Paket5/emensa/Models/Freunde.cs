using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Freunde
    {
        public int Nutzer { get; set; }
        public int Freund { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Benutzer FreundNavigation { get; set; }
        public virtual Benutzer NutzerNavigation { get; set; }
    }
}
