namespace CountryService.Grpc.IntegrationTests.HealthChecksTests;

public class LiveCheckTests(CountryGrpcIntegrationFixture fixture) : HealthChecksTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnOk()
    {
        // Act
        var response = await HttpClient.GetAsync("/health/live", CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync(CancellationToken);
        content.Should().Be(nameof(HealthStatus.Healthy));
    }
    
    [Fact]
    public async Task ShouldReturnOk_WhenDatabaseIsNotReady()
    {
        // Arrange
        await Fixture.Postgres.StopAsync(CancellationToken);

        // Act
        var response = await HttpClient.GetAsync("/health/live", CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync(CancellationToken);
        content.Should().Be(nameof(HealthStatus.Healthy));
    }
}