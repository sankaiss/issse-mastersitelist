using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{


    public class Skrivare
    {
        public int Id { get; set; }
        public string? Gatuadress { get; set; }
        public string? Ort { get; set; }
        public string? ShareName { get; set; }
        public string? Port { get; set; }
        public string? Modell { get; set; }
    }
}