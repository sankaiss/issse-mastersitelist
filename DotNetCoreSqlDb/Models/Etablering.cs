using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{


    public class Etablering
    {
        public int Id { get; set; }
        public string? KontorSite { get; set; }
        public string? AteaKonsult { get; set; }
        public string? Gatuadress { get; set; }
        public string? ÄrendeNrISS { get; set; }
        public string? Ort { get; set; }
        public string? ISSKontaktperson { get; set; }
        public string? EpostISSKontaktperson { get; set; }
        public string? TelefonISSKontaktperson { get; set; }
        public string? TeliaUppkoppling { get; set; }
        public string? ÖvrigInfo { get; set; }
        public string? KlartSenast { get; set; }
        public string? Status { get; set; }
    }
}