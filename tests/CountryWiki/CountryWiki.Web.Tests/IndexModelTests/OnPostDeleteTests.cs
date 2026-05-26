namespace CountryWiki.Web.Tests.IndexModelTests;

public sealed class OnPostDeleteTests : IndexModelTestsBase
{
    [Fact]
    public async Task ShouldDeleteCountry_AndRedirectToIndex()
    {
        // Arrange
        const int countryId = 1;
        CountryService.DeleteAsync(countryId, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId, CancellationToken);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("./Index");

        await CountryService.Received(1).DeleteAsync(countryId, Arg.Any<CancellationToken>());
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
        CountryService.DeleteAsync(countryId, Arg.Any<CancellationToken>()).Returns(_ => throw exception);
        CountryService.GetAllAsync(Arg.Any<CancellationToken>()).Returns(countries);

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId, CancellationToken);

        // Assert
        result.Should().BeOfType<PageResult>();
        IndexModel.ErrorMessage.Should().Be(exception.Message);
        IndexModel.Countries.Should().BeEquivalentTo(countries);

        await CountryService.Received(1).DeleteAsync(countryId, Arg.Any<CancellationToken>());
        await CountryService.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldKeepDeleteError_WhenReloadingCountriesFails()
    {
        // Arrange
        const int countryId = 1;
        var deleteException = TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.ServiceUnavailable);
        CountryService.DeleteAsync(countryId, Arg.Any<CancellationToken>()).Returns(_ => throw deleteException);
        CountryService.GetAllAsync(Arg.Any<CancellationToken>()).Returns<IEnumerable<CountryModel>>(_ =>
            throw TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.ServiceUnavailable));

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId, CancellationToken);

        // Assert
        result.Should().BeOfType<PageResult>();
        IndexModel.ErrorMessage.Should().Be(deleteException.Message);
        IndexModel.Countries.Should().BeEmpty();

        await CountryService.Received(1).DeleteAsync(countryId, Arg.Any<CancellationToken>());
        await CountryService.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldPassCancellationTokenToCountryService()
    {
        // Arrange
        const int countryId = 10;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        CountryService.DeleteAsync(countryId, cancellationToken).Returns(Task.CompletedTask);

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId, cancellationToken);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("./Index");

        await CountryService.Received(1).DeleteAsync(countryId, cancellationToken);
    }
}