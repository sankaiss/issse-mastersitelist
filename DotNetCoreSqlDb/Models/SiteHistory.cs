using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class SiteHistory
    {
        public int Id { get; set; }

        [Required]
        public int SiteId { get; set; }  // FK till Site-tabellen

        [Required]
        public DateTime ChangedDate { get; set; }

        [Required, DisplayName("Fält Namn")]
        public string FieldName { get; set; }  // Namnet på fältet som ändrades

        [DisplayName("Gammalt Värde")]
        public string OldValue { get; set; }  // Det gamla värdet

        [DisplayName("Nytt Värde")]
        public string NewValue { get; set; }  // Det nya värdet

        // Navigerings-egenskap för Site
        public virtual Site Site { get; set; }  
    }
}
