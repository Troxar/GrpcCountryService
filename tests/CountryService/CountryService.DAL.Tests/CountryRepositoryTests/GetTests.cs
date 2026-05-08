namespace CountryService.DAL.Tests.CountryRepositoryTests;

public class GetTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnCountryModel_WhenCountryExists()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);
        var country = TestDataFactory.CreateCountry();
        country.CountryLanguages = new List<CountryLanguage>
        {
            new() { LanguageId = 1 },
            new() { LanguageId = 2 }
        };

        await context.Countries.AddAsync(country, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);

        // Act
        var result = await repository.GetAsync(country.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(country.Id);
        result.Name.Should().Be(country.Name);
        result.Description.Should().Be(country.Description);
        result.FlagUri.Should().Be(country.FlagUri);
        result.CapitalCity.Should().Be(country.CapitalCity);
        result.Anthem.Should().Be(country.Anthem);
        result.Languages.Should().BeEquivalentTo("English", "French");
    }

    [Fact]
    public async Task ShouldReturnNull_WhenCountryDoesNotExist()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);

        // Act
        var result = await repository.GetAsync(999);

        // Assert
        result.Should().BeNull();
    }
}