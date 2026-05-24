namespace CountryWiki.Web.Extensions;

public static class EndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        public IEndpointRouteBuilder MapCountryWikiEndpoints()
        {
            builder.MapRazorPages();
            builder.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("live")
            });
            builder.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("ready")
            });

            return builder;
        }
    }
}