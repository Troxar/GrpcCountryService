namespace CountryWiki.Web.Tests.EditModelTests;

public sealed class OnGetTests : EditModelTestsBase
{
    [Fact]
    public async Task ShouldFillPageModel_WhenCountryExists()
    {
        // Arrange
        var country = TestDataFactory.CreateCountryModel(1);
        CountryService.GetAsync(country.Id).Returns(country);

        // Act
        await EditModel.OnGetAsync(country.Id);

        // Assert
        EditModel.CountryName.Should().Be(country.Name);
        EditModel.CountryToUpdate.Should().BeEquivalentTo(country, options => options.ExcludingMissingMembers());

        await CountryService.Received(1).GetAsync(country.Id);
    }

    [Fact]
    public async Task ShouldLeavePageModelDefault_WhenCountryDoesNotExist()
    {
        // Arrange
        const int countryId = 999;
        CountryService.GetAsync(countryId).Returns((CountryModel?)null);

        // Act
        await EditModel.OnGetAsync(countryId);

        // Assert
        EditModel.CountryName.Should().BeEmpty();
        EditModel.CountryToUpdate.Id.Should().Be(0);
        EditModel.CountryToUpdate.Description.Should().BeNullOrEmpty();

        await CountryService.Received(1).GetAsync(countryId);
    }
}