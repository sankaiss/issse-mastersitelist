﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Data
{
    public class MyDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public MyDatabaseContext (DbContextOptions<MyDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<DotNetCoreSqlDb.Models.Site> Site { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.IPPlan> IPPlan { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.IPPlanLog> IPPlanLogs { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.Etablering> Etableringar { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.SiteLog> Sitelogs { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.Kassa> Kassas { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.Skrivare> Skrivare { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.PrinterLog> PrinterLogs { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.KassaLog> KassaLogs { get; set; } = default!; 
        //public DbSet<DotNetCoreSqlDb.Models.SiteImage> SiteImages { get; set; } = default!;            

    }
}
