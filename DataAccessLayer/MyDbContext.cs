﻿using DataAccessLayer.ModelsDb;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class MyDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Uritus> Uritus { get; set; }
        public DbSet<Osaleja> Osaleja { get; set; }
        public DbSet<Eraisik> Eraisik { get; set; }
        public DbSet<Ettevote> Ettevote { get; set; }
        public DbSet<Makseviis> Makseviis { get; set; }

        public DbSet<EraisikOsaleja> EraisikOsaleja { get; set; }
        public DbSet<EttevoteOsaleja> EttevoteOsaleja { get; set; }
    }
}
