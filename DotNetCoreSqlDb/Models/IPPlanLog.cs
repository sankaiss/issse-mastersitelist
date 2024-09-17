using System;

namespace DotNetCoreSqlDb.Models
{
    public class IPPlanLog
    {
        public int Id { get; set; }
        public int IPPlanId { get; set; } // Referens till IPPlan
        public string? PropertyName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangeDate { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
