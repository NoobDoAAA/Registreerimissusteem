﻿using DataAccessLayer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class MyDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Uritus> Uritused { get; set; }
    }
}
