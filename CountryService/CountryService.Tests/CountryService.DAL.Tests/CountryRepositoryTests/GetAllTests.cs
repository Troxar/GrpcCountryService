namespace CountryService.DAL.Tests.CountryRepositoryTests;

public class GetAllTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnAllCountries()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);
        var countries = new[]
        {
            TestDataFactory.CreateCountry(),
            TestDataFactory.CreateCountry()
        };

        await context.Countries.AddRangeAsync(countries, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        var countryModels = result as CountryModel[] ?? result.ToArray();
        countryModels.Should().HaveCount(countries.Length);

        foreach (var country in countries)
            countryModels.Should().Contain(x =>
                x.Id == country.Id
                && x.Name == country.Name
                && x.Description == country.Description
                && x.FlagUri == country.FlagUri
                && x.CapitalCity == country.CapitalCity
                && x.Anthem == country.Anthem);
    }
}