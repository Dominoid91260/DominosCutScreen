using DominosCutScreen.Shared;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace DominosCutScreen.Server.Models
{
    public class CutBenchContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CutBenchContext> _logger;

        public CutBenchContext(IConfiguration config, ILogger<CutBenchContext> logger)
        {
            _config = config;
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(_config.GetConnectionString("SqliteDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SettingsService>()
                .HasData(new SettingsService()
                {
                    SettingsServiceId = 1,
                    MakelineServer = "http://localhost:59108",
                    MakelineCode = 2,
                    OvenTime = 300,
                    GraceTime = 90,
                    AlertInterval = 150,
                    FetchInterval = 5
                });

            modelBuilder.Entity<SettingsService>()
                .OwnsOne(s => s.QuietTime)
                .HasData(new object[]
                {
                    new { SettingsServiceId = 1, IsEnabled = false, Start = new TimeOnly(), End = new TimeOnly() }
                });

            modelBuilder.Entity<SettingsService>()
                .OwnsOne(s => s.TimedOrderAlarm)
                .HasData(new object[]
                {
                    new { SettingsServiceId = 1, IsEnabled = false, SecondsPerPizza = 15, MinPizzaThreshold = 7 }
                });
        }

        public DbSet<SettingsService> Settings {
            get => Set<SettingsService>();
        }

        public SettingsService GetSettings()
        {
            if (_actualSettings == null)
            {
                _actualSettings = Settings.FirstOrDefault();
            }

            return _actualSettings;
        }
        private SettingsService _actualSettings;
    }
}
