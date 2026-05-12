namespace CountryService.Grpc.IntegrationTests.IntegrationTests;

public class UpdateTests(CountryGrpcIntegrationFixture fixture) : IntegrationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldUpdateCountry_WhenCountryExists()
    {
        // Arrange
        var country = TestDataFactory.CreateCountry();
        await SeedCountriesAsync(country);

        var request = TestDataFactory.CreateCountryUpdateRequest(country.Id);

        // Act
        var reply = await Fixture.Client.UpdateAsync(request, cancellationToken: CancellationToken);

        // Assert
        reply.Should().NotBeNull();

        var updatedCountry = await FindCountryAsync(country.Id);
        updatedCountry.Should().NotBeNull();
        updatedCountry.Description.Should().Be(request.Description);
        updatedCountry.UpdateDate.Should().NotBeNull()
            .And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenCountryDoesNotExist()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryUpdateRequest(999);

        // Act
        var act = async () => await Fixture.Client.UpdateAsync(request, cancellationToken: CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Which.Status.Detail.Should().Contain("Country with Id 999 hasn't been updated");
    }
}