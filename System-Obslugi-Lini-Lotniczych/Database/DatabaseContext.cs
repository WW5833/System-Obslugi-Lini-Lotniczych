using LotSystem.Database.Models;
using LotSystem.Logger;
using LotSystem.Logger.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LotSystem.Database;

public sealed class DatabaseContext : DbContext
{
    private readonly ILogger _logger;
    private readonly string _path;

    public DatabaseContext(ILogger logger, string path = "db.db")
    {
        _logger = logger;
        _path = path;
    }

    public DbSet<Airport> Airports { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<UserSession> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_path};");
        optionsBuilder.LogTo(_logger.Database, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}

#if DEBUG
public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    DatabaseContext IDesignTimeDbContextFactory<DatabaseContext>.CreateDbContext(string[] args)
    {
        return new DatabaseContext(new DebugLogger(), "bin/Debug/net6.0/db.db");
    }
}
#endif