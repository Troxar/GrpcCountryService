namespace CountryService.Grpc.IntegrationTests.HealthChecksTests;

public class ReadyTests(CountryGrpcIntegrationFixture fixture) : HealthChecksTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnOk()
    {
        // Act
        var response = await HttpClient.GetAsync("/health/ready", CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync(CancellationToken);
        content.Should().Be(nameof(HealthStatus.Healthy));
    }

    [Fact]
    public async Task ShouldReturnServiceUnavailable_WhenDatabaseIsNotReady()
    {
        // Arrange
        await Fixture.Postgres.StopAsync(CancellationToken);

        // Act
        var response = await HttpClient.GetAsync("/health/ready", CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

        var content = await response.Content.ReadAsStringAsync(CancellationToken);
        content.Should().Be(nameof(HealthStatus.Unhealthy));
    }
}