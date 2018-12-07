using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emensa.Models
{
    public partial class Kommentare
    {
        [Key]
        public int Id { get; set; }
        public int FkStudentId { get; set; }
        public int? FkzuMahlzeit { get; set; }
        public string Bemerkung { get; set; }
        public int Bewertung { get; set; }

        public virtual Student FkStudent { get; set; }
        public virtual Mahlzeiten FkzuMahlzeitNavigation { get; set; }
    }
}
