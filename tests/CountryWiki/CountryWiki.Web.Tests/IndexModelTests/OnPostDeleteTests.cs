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
}