using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class IPPlan
    {
        public int ID { get; set; }
        public string? Nät { get; set; }
        public string? Mask { get; set; }
        public string? MGMT { get; set; }
        public string? WLAN { get; set; }
        public string? LAN { get; set; }
        public string? Site { get; set; }
        public string? GammalSite { get; set; }
        public string? Status { get; set; }
        public string? PRNT { get; set; }
    

        public DateTime LastUpdatedDate { get; set; } 
    }
}
