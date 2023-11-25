
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
        public int Id { get; set; }
        [Required]
        public int SiteID { get; set; }

        [Required]
        public string ImageUrl { get; set; }
     

}
}