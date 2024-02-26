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
                    FetchInterval = 5,
                    PulseApiServer = "http://pulseapi"
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

            modelBuilder.Entity<SettingsService>()
                .HasMany(s => s.PostBakes);

            modelBuilder.Entity<PostBake>()
                .HasData(
                    new { SettingsServiceId = 1, ReceiptCode = "*", ToppingCode = "SPO", ToppingDescription = "Spring Onion", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "B", ToppingCode = "BtrChk", ToppingDescription = "Butter Chicken Sce", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "CS", ToppingCode = "CeSpr", ToppingDescription = "Cheese Sprinkle", IsEnabled = true }, // Not a real topping so theres no topping code
                    new { SettingsServiceId = 1, ReceiptCode = "F", ToppingCode = "FRANKS", ToppingDescription = "Franks Hot Sce", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "GB", ToppingCode = "GBUTT", ToppingDescription = "Garlic Butter", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "HB", ToppingCode = "HICBBQ", ToppingDescription = "Hickory BBQ", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "HO", ToppingCode = "HOLLAND", ToppingDescription = "Hollandaise", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "M", ToppingCode = "My", ToppingDescription = "Mayonnaise", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "P", ToppingCode = "PERI", ToppingDescription = "Peri Peri Sce", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "PA", ToppingCode = "PARMSC", ToppingDescription = "Garlc Parm Sce", IsEnabled = true },
                    new { SettingsServiceId = 1, ReceiptCode = "T", ToppingCode = "TOMCAP", ToppingDescription = "Tom Caps Sce", IsEnabled = true }
                );
        }

        public DbSet<PostBake> PostBakes { get; set; }
        public DbSet<SettingsService> Settings {
            get => Set<SettingsService>();
        }

        public SettingsService GetSettings()
        {
            if (_actualSettings == null)
            {
                _actualSettings = Settings.Include(s => s.PostBakes).FirstOrDefault();
            }

            return _actualSettings;
        }
        private SettingsService _actualSettings;
    }
}
