
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
public class KassaLog
{
    public int Id { get; set; }
    public int KassaId { get; set; }
    public string? PropertyName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangeDate { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
}
}