namespace CountryWiki.Web.IntegrationTests.HealthChecksTests.GrpcIsNotReadyTests;

public class LiveCheckTests
{
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task ShouldReturnOk_WhenCountryServiceGrpcIsNotReady()
    {
        // Arrange
        using var countryServiceHandler = new UnavailableCountryServiceHandler();
        await using var factory = new CountryWikiWebFactory(countryServiceHandler, new TestLoggerProvider());
        using var httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await httpClient.GetAsync("/health/live", _cancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync(_cancellationToken);
        content.Should().Be(nameof(HealthStatus.Healthy));
    }
}