using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class SiteHistory
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public Site Site { get; set; } // Foreign key relation
        public string PropertyName { get; set; }
        public DateTime ChangedOn { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

}
