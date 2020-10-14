﻿using PyrotechnicShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyrotechnicShop.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Pyrotechnics> Pyrotechnics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EFDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}