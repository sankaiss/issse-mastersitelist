using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreSqlDb.Models
{
    public class ImageReference
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public int SiteID { get; set; } // Främmande nyckel för att koppla till Site
        public Site Site { get; set; }
    }
}