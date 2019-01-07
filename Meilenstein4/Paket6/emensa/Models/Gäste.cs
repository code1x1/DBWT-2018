using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Gäste
    {
        public string Grund { get; set; }
        public DateTime Ablaufdatum { get; set; }
        public int FkBenutzer { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Benutzer FkBenutzerNavigation { get; set; }
    }
}
