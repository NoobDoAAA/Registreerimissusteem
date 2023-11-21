using DataAccessLayer.ModelsDb;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class MyDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Uritus> Uritus { get; set; }
        public DbSet<Osaleja> Osaleja { get; set; }
    }
}
