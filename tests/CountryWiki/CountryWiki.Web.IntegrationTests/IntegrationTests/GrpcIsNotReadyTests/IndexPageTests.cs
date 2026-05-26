namespace CountryWiki.Web.IntegrationTests.IntegrationTests.GrpcIsNotReadyTests;

public sealed class IndexPageTests
{
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task ShouldShowServiceUnavailableMessage_WhenCountryServiceGrpcIsNotReady()
    {
        // Arrange
        using var countryServiceHandler = new UnavailableCountryServiceHandler();
        await using var factory = new CountryWikiWebFactory(countryServiceHandler, new TestLoggerProvider());
        using var httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await httpClient.GetAsync("/", _cancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync(_cancellationToken);
        content.Should().Contain("Country service is temporarily unavailable");
        content.Should().NotContain("RpcException");
        content.Should().NotContain("StatusCode");
    }
}