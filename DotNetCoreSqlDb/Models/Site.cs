using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DotNetCoreSqlDb.Models
{
    public class Site
    {
        public int ID { get; set; }
        public string? Ort { get; set; }
        public string? Gatuadress { get; set; }
        public string? SiteTyp { get; set; }
        public string? GammalAdressEfterFlytt { get; set; }
        public string? Leverantör { get; set; }
        public string? Status { get; set; }
        public string? NätverkskapacitetMbps { get; set; }
        public int? NätverkskapacitetGbps { get; set; } 
        public string? KontaktNamn { get; set; }
        public string? ISSKontorSite { get; set; }
        public string? Mobilnr { get; set; }
        public string? Epostadress { get; set; }
        public string? WANUplink { get; set; }
        public string? AntalEnheter { get; set; }
        public string? Sitestorlek { get; set; }
        public string? Kommentarer { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsArchived { get; set; } = false;
   


    }
}
