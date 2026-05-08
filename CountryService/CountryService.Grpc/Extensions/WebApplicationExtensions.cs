namespace CountryService.Grpc.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var countryContext = scope.ServiceProvider.GetRequiredService<CountryContext>();
        countryContext.Database.Migrate();
    }
}