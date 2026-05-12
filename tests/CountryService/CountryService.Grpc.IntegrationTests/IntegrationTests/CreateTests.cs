namespace CountryService.Grpc.IntegrationTests.IntegrationTests;

public class CreateTests(CountryGrpcIntegrationFixture fixture) : IntegrationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldCreateCountriesAndReturnReplies_WhenRequestStreamContainsCountries()
    {
        // Arrange
        var requests = new[]
        {
            TestDataFactory.CreateCountryCreateRequest(),
            TestDataFactory.CreateCountryCreateRequest()
        };

        using var call = Fixture.Client.Create(cancellationToken: CancellationToken);

        // Act
        foreach (var request in requests)
            await call.RequestStream.WriteAsync(request, CancellationToken);

        await call.RequestStream.CompleteAsync();

        var replies = new Dictionary<string, CountryCreateReply>();
        await foreach (var reply in call.ResponseStream.ReadAllAsync(CancellationToken))
            replies.Add(reply.Name, reply);

        // Assert
        replies.Should().HaveCount(requests.Length);

        foreach (var request in requests)
        {
            var reply = replies.GetValueOrDefault(request.Name);
            reply.Should().NotBeNull();
            reply.Id.Should().BeGreaterThan(0);
        }

        var countriesCount = await CountCountriesAsync();
        countriesCount.Should().Be(requests.Length);
    }
}