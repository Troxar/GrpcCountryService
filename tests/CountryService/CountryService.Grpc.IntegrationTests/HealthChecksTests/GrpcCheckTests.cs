namespace CountryService.Grpc.IntegrationTests.HealthChecksTests;

public class GrpcCheckTests(CountryGrpcIntegrationFixture fixture) : HealthChecksTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnServing_WhenGeneralGrpcHealthCheck()
    {
        // Arrange
        var client = new Health.HealthClient(GrpcChannel);
        var request = new HealthCheckRequest();

        // Act
        var response = await client.CheckAsync(request, cancellationToken: CancellationToken);

        // Assert
        response.Status.Should().Be(HealthCheckResponse.Types.ServingStatus.Serving);
    }

    [Fact]
    public async Task ShouldReturnServing_WhenCountryServiceGrpcHealthCheck()
    {
        // Arrange
        var client = new Health.HealthClient(GrpcChannel);
        var request = new HealthCheckRequest
        {
            Service = v1.CountryService.Descriptor.FullName
        };

        // Act
        var response = await client.CheckAsync(request, cancellationToken: CancellationToken);

        // Assert
        response.Status.Should().Be(HealthCheckResponse.Types.ServingStatus.Serving);
    }
}