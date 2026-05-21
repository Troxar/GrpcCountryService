namespace CountryWiki.Web.HealthChecks;

public class CountryServiceGrpcHealthCheck : IHealthCheck
{
    private readonly Health.HealthClient _healthClient;

    public CountryServiceGrpcHealthCheck(Health.HealthClient healthClient)
    {
        _healthClient = healthClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HealthCheckRequest
                { Service = CountryService.Grpc.v1.CountryService.Descriptor.FullName };
            var response = await _healthClient.CheckAsync(request, deadline: DateTime.UtcNow.AddSeconds(3),
                cancellationToken: cancellationToken);
            return response.Status == HealthCheckResponse.Types.ServingStatus.Serving
                ? HealthCheckResult.Healthy("CountryService gRPC endpoint is serving")
                : HealthCheckResult.Unhealthy($"CountryService gRPC endpoint status is {response.Status}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("CountryService gRPC health check failed", ex);
        }
    }
}