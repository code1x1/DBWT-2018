using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Html;


namespace emensa.Models
{
    public partial class Zutaten
    {
        public string bioLogoString(){
            if(this.Bio == 1){
                return $@"<img class='bio' alt='bio logo' src='/images/bio-logo.png' />";
            }
            return "";
        }
        public Zutaten()
        {
            MahlzeitenZutaten = new HashSet<MahlzeitenZutaten>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0, 1, 
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public byte Bio { get; set; }
        [Range(0, 1, 
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public byte Vegetarisch { get; set; }
        [Range(0, 1, 
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public byte Vegan { get; set; }
        [Range(0, 1, 
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public byte Glutenfrei { get; set; }

        public virtual ICollection<MahlzeitenZutaten> MahlzeitenZutaten { get; set; }
    }
}
