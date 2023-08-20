using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
public class SiteLog
{
    public int ID { get; set; }
    public int SiteId { get; set; } 
    public string? PropertyName { get; set; } 
    public string? OldValue { get; set; } 
    public string? NewValue { get; set; } 
    public DateTime ChangeDate { get; set; } 
    public string? UserId { get; set; }
    public string? UserName { get; set; }  

}

}
