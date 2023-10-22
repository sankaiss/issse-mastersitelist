
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreSqlDb.Models
{
    public class SiteImage
    {
        public int ID { get; set; }
        public int SiteID { get; set; }
        public string? FileName { get; set; }        
        public DateTime UploadDate { get; set; }
}
}