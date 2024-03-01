using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HeizungUeberwachung;

public class Context : DbContext
{
    public DbSet<TemperatureLog> TemperatureLogs { get; set; }
    public DbSet<EventLog> EventLogs { get; set; }

    public string DbPath { get; }
    public Context()
    {
        DbPath = Path.Join(".", "db", "logging.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var eventConverter = new EnumToStringConverter<Event>();

        modelBuilder.Entity<EventLog>()
            .Property(e => e.Event)
            .HasConversion(eventConverter);
    }
}

public record TemperatureLog(DateTime DateTime, double Temperature, int? Id = null);

public record EventLog(DateTime DateTime, Event Event, int? Id = null);