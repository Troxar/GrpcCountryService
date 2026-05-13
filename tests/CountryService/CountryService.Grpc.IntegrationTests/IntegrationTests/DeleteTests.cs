namespace CountryService.Grpc.IntegrationTests.IntegrationTests;

public class DeleteTests(CountryGrpcIntegrationFixture fixture) : IntegrationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldDeleteCountry_WhenCountryExists()
    {
        // Arrange
        var country = TestDataFactory.CreateCountry();
        await SeedCountriesAsync(country);

        var request = TestDataFactory.CreateCountryIdRequest(country.Id);

        // Act
        var reply = await Fixture.Client.DeleteAsync(request, cancellationToken: CancellationToken);

        // Assert
        reply.Should().NotBeNull();

        var existingCountry = await FindCountryAsync(country.Id);
        existingCountry.Should().BeNull();
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenCountryDoesNotExist_WithCorrelationIdTrailer()
    {
        // Arrange
        const string correlationId = "test-correlation-id";
        var metadata = new Metadata
        {
            { MetadataNames.CorrelationId, correlationId }
        };
        var request = TestDataFactory.CreateCountryIdRequest(999);

        // Act
        var act = async () => await Fixture.Client.DeleteAsync(request, metadata, cancellationToken: CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Which.Status.Detail.Should().Contain("Country with Id 999 hasn't been deleted");
        exception.Which.Trailers.GetValue(MetadataNames.CorrelationId).Should().Be(correlationId);
    }

    [Fact]
    public async Task ShouldReturnDefaultCorrelationIdTrailer_WhenCorrelationIdIsMissing()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(999);

        // Act
        var act = async () => await Fixture.Client.DeleteAsync(request, cancellationToken: CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Which.Trailers.GetValue(MetadataNames.CorrelationId).Should().Be($"no-{MetadataNames.CorrelationId}");
    }
}