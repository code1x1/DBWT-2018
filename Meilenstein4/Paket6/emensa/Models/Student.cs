using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Student
    {
        public Student()
        {
            Kommentare = new HashSet<Kommentare>();
        }
        [Key]
        public int StudentId { get; set; }
        public string Studiengang { get; set; }
        public int Matrikelnummer { get; set; }
        public int FkFhange { get; set; }

        public virtual FhAngehörige FkFhangeNavigation { get; set; }
        public virtual ICollection<Kommentare> Kommentare { get; set; }
    }
}
