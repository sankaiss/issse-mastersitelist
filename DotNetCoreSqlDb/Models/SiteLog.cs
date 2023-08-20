using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
public class SiteLog
{
    public int ID { get; set; }
    public int SiteId { get; set; } // Referens till den Site som ändrades
    public string? PropertyName { get; set; } // Namnet på egenskapen som ändrades
    public string? OldValue { get; set; } // Tidigare värde
    public string? NewValue { get; set; } // Nytt värde
    public DateTime ChangeDate { get; set; } // Datum/tid för ändringen
}

}
