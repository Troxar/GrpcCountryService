namespace CountryWiki.Web.Tests.IndexModelTests;

public sealed class OnPostDeleteTests : IndexModelTestsBase
{
    [Fact]
    public async Task ShouldDeleteCountry_AndRedirectToIndex()
    {
        // Arrange
        const int countryId = 1;
        CountryService.DeleteAsync(countryId).Returns(Task.CompletedTask);

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("./Index");

        await CountryService.Received(1).DeleteAsync(countryId);
    }

    [Fact]
    public async Task ShouldReturnPageWithErrorMessage_WhenCountryServiceThrowsInternalError()
    {
        // Arrange
        const int countryId = 1;
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(2)
        };
        var exception = TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.ServiceUnavailable);
        CountryService.DeleteAsync(countryId).Returns(_ => throw exception);
        CountryService.GetAllAsync().Returns(countries);

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId);

        // Assert
        result.Should().BeOfType<PageResult>();
        IndexModel.ErrorMessage.Should().Be(exception.Message);
        IndexModel.Countries.Should().BeEquivalentTo(countries);

        await CountryService.Received(1).DeleteAsync(countryId);
        await CountryService.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task ShouldKeepDeleteError_WhenReloadingCountriesFails()
    {
        // Arrange
        const int countryId = 1;
        var deleteException = TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.ServiceUnavailable);
        CountryService.DeleteAsync(countryId).Returns(_ => throw deleteException);
        CountryService.GetAllAsync().Returns<IEnumerable<CountryModel>>(_ =>
            throw TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.ServiceUnavailable));

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId);

        // Assert
        result.Should().BeOfType<PageResult>();
        IndexModel.ErrorMessage.Should().Be(deleteException.Message);
        IndexModel.Countries.Should().BeEmpty();

        await CountryService.Received(1).DeleteAsync(countryId);
        await CountryService.Received(1).GetAllAsync();
    }
}