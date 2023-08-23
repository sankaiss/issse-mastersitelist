using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{


    public class Kassa
    {
        public int Id { get; set; }
        public string? KontorSite { get; set; }
        public string? Gatuadress { get; set; }
        public string? ÄrendeNrISS { get; set; }
        public string? Ort { get; set; }
        public string? ISSKontaktperson { get; set; }
        public string? EpostISSKontaktperson { get; set; }
        public string? TelefonISSKontaktperson { get; set; }
        public string? KassaTyp { get; set; }
        public string? Uppkopling { get; set; }
        public string? Leveraantör { get; set; }
        public string? Övrigt { get; set; }
    }
}