using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

public class SiteImage
{
    public int Id { get; set; }

    [Required]
    public string? SiteId { get; set; } // Länka bilden till en specifik Site med ett ID

    [Required]
    public string? ImageUrl { get; set; } // Länken till den uppladdade bilden

    // Lägg till fler egenskaper om det behövs (t.ex. beskrivning, datum, osv.)
}
