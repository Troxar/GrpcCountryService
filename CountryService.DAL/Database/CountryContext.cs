namespace CountryService.DAL.Database;

public class CountryContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: take from configuration
        const string connectionString =
            "Host=localhost;Port=9595;Database=CountryService;Username=postgres;Password=secretpassword";
        optionsBuilder.UseNpgsql(connectionString);
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<CountryLanguage> CountryLanguages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CountryContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}