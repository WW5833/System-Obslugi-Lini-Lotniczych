using LotSystem.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace LotSystem.Database
{
    public sealed class DatabaseContext : DbContext
    {
        public DbSet<Airport> Airports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db.db;");
        }
    }
}