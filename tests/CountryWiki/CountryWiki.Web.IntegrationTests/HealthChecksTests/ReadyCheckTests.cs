namespace CountryWiki.Web.IntegrationTests.HealthChecksTests;

public class ReadyCheckTests(CountryWikiIntegrationFixture fixture) : HealthChecksTestsBase(fixture)
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
}