using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace emensa.ViewModels {
    public class BildDatei
{
    // TODO: View Model für Bild Upload

    [Key]
    public int Id { get; set; }
    [Required]
    public string AltText { get; set; }
    [Required]
    public string Titel { get; set; }
    [Required]
    public string Copyright { get; set; }
    [Required]
    public IFormFile Binärdaten { get; set; }
}
}