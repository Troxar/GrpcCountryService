namespace CountryWiki.Web.Tests.IndexModelTests;

public sealed class OnGetTests : IndexModelTestsBase
{
    [Fact]
    public async Task ShouldLoadCountries_WhenCountryServiceSucceeds()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1),
            TestDataFactory.CreateCountryModel(2)
        };
        CountryService.GetAllAsync(Arg.Any<CancellationToken>()).Returns(countries);

        // Act
        await IndexModel.OnGetAsync(CancellationToken);

        // Assert
        IndexModel.Countries.Should().BeEquivalentTo(countries);
        await CountryService.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldNotLoadCountries_WhenUploadProcessingIsInProgress()
    {
        // Arrange
        GlobalOptions.ProcessingUpload = true;

        // Act
        await IndexModel.OnGetAsync(CancellationToken);

        // Assert
        IndexModel.Countries.Should().BeEmpty();
        await CountryService.DidNotReceive().GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldSetErrorMessageAndEmptyCountries_WhenCountryServiceThrowsServiceUnavailable()
    {
        // Arrange
        var exception = TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.ServiceUnavailable);
        CountryService.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException<IEnumerable<CountryModel>>(exception));

        // Act
        await IndexModel.OnGetAsync(CancellationToken);

        // Assert
        IndexModel.Countries.Should().BeEmpty();
        IndexModel.ErrorMessage.Should().Be(exception.Message);
    }
}