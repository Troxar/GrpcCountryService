namespace CountryService.Grpc.Extensions;

public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        public async Task ApplyMigrationsIfConfiguredAsync()
        {
            var options = app.Services.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            if (!options.ApplyMigrationsOnStartup)
                return;

            await using var scope = app.Services.CreateAsyncScope();
            var countryContext = scope.ServiceProvider.GetRequiredService<CountryContext>();
            await countryContext.Database.MigrateAsync();
        }

        public void MapCountryServiceEndpoints()
        {
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client");
            app.MapGrpcService<CountryGrpcService>();

            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = context => context.Tags.Contains("live")
            });
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = context => context.Tags.Contains("ready")
            });
            app.MapGrpcHealthChecksService();

            if (app.Environment.IsDevelopment())
                app.MapGrpcReflectionService();
        }
    }
}