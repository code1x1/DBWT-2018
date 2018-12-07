using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class FhAngehörige
    {
        public FhAngehörige()
        {
            GehörtZuFachbereiche = new HashSet<GehörtZuFachbereiche>();
            Mitarbeiter = new HashSet<Mitarbeiter>();
            Student = new HashSet<Student>();
        }
        [Key]
        public int Id { get; set; }
        public int FkBenutzer { get; set; }

        public virtual Benutzer FkBenutzerNavigation { get; set; }
        public virtual ICollection<GehörtZuFachbereiche> GehörtZuFachbereiche { get; set; }
        public virtual ICollection<Mitarbeiter> Mitarbeiter { get; set; }
        public virtual ICollection<Student> Student { get; set; }
    }
}
