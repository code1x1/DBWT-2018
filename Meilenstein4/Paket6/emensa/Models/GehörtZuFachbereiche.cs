using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class GehörtZuFachbereiche
    {
        public int FkFachbereiche { get; set; }
        public int FkFhange { get; set; }
        [Key]
        public int Id { get; set; }

        public virtual Fachbereiche FkFachbereicheNavigation { get; set; }
        public virtual FhAngehörige FkFhangeNavigation { get; set; }
    }
}
