namespace CountryService.DAL.Database;

public sealed class CountryContextFactory : IDesignTimeDbContextFactory<CountryContext>
{
    public CountryContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString(args);

        var options = new DbContextOptionsBuilder<CountryContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new CountryContext(options);
    }

    private static string GetConnectionString(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.DesignTime.json", true)
            .AddEnvironmentVariables()
            .Build();

        return configuration.GetConnectionString("CountryService")
               ?? throw new InvalidOperationException(
                   "Connection string 'CountryService' is not configured. " +
                   "Set it in appsettings.DesignTime.json or environment variable 'ConnectionStrings__CountryService'");
    }
}