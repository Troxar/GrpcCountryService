namespace CountryService.DAL.Tests.CountryRepositoryTests;

public sealed class GetAllTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
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
        result.Should().BeEquivalentTo(countries, options => options.ExcludingMissingMembers());
    }
}