using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema; // Importera detta namespace

namespace DotNetCoreSqlDb.Models
{
    public class SiteImage
    {
        public int Id { get; set; }

        [Required]
        [Column("SiteID")] // Ange det faktiska kolumnnamnet i din databas
        public string? SiteId { get; set; } // Länka bilden till en specifik Site med ett ID

        [Required]
        public string? ImageUrl { get; set; } // Länken till den uppladdade bilden

        // Lägg till fler egenskaper om det behövs (t.ex. beskrivning, datum, osv.)
    }
}
