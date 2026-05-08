using Microsoft.Extensions.Configuration;

namespace CountryService.DAL.Database;

public class CountryContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CountryContext(DbContextOptions<CountryContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("CountryService");
        optionsBuilder.UseNpgsql(connectionString);
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<CountryLanguage> CountryLanguages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CountryContext).Assembly);
        SeedLanguages(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void SeedLanguages(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>().HasData(
            new Language { Id = 1, Name = "English" },
            new Language { Id = 2, Name = "French" },
            new Language { Id = 3, Name = "Spanish" }
        );
    }
}