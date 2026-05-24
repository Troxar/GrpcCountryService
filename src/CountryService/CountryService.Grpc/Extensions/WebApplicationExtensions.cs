namespace CountryService.Grpc.Extensions;

public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        public void ApplyMigrations()
        {
            using var scope = app.Services.CreateScope();
            var countryContext = scope.ServiceProvider.GetRequiredService<CountryContext>();
            countryContext.Database.Migrate();
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