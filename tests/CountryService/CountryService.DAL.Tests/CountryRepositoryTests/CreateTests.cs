namespace CountryService.DAL.Tests.CountryRepositoryTests;

public sealed class CreateTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
{
    [Fact]
    public async Task ShouldCreateCountry_AndReturnGeneratedId()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);
        var model = TestDataFactory.CreateCountryModel(null, 1, 2);

        // Act
        var id = await repository.CreateAsync(model, CancellationToken);

        // Assert
        id.Should().BeGreaterThan(0);

        var country = await context.Countries
            .AsNoTracking()
            .Include(x => x.CountryLanguages)
            .SingleAsync(x => x.Id == id, CancellationToken);

        country.Should().BeEquivalentTo(model, options => options
            .Excluding(x => x.Languages));
        country.CreateDate.Should().NotBe(default);
        country.UpdateDate.Should().BeNull();
        country.CountryLanguages.Should().NotBeNullOrEmpty();
        country.CountryLanguages.Select(x => x.LanguageId).Should().BeEquivalentTo(model.Languages);
    }

    [Fact]
    public async Task ShouldThrowCountryAlreadyExistsException_WhenNameAlreadyExists()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var existingCountry = TestDataFactory.CreateCountry();
        context.Countries.Add(existingCountry);

        await context.SaveChangesAsync(CancellationToken);

        var repository = new CountryRepository(context);
        var model = TestDataFactory.CreateCountryModel(existingCountry.Name);

        // Act
        var act = async () => await repository.CreateAsync(model, CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<CountryAlreadyExistsException>();
        exception.Which.Name.Should().Be(existingCountry.Name);

        var countriesCount = await context.Countries.CountAsync(x => x.Name == existingCountry.Name, CancellationToken);
        countriesCount.Should().Be(1);
    }

    [Fact]
    public async Task ShouldThrowCountryAlreadyExistsException_WhenUniqueConstraintViolated()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var countryName = Guid.NewGuid().ToString();
        var options = Fixture.CreateContextOptions();
        var interceptor = new InsertDuplicateCountryBeforeSaveInterceptor(options, countryName);

        await using var contextWithInterceptor = Fixture.CreateContext(interceptor);

        var repository = new CountryRepository(contextWithInterceptor);
        var model = TestDataFactory.CreateCountryModel(countryName);

        // Act
        var act = async () => await repository.CreateAsync(model, CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<CountryAlreadyExistsException>();
        exception.Which.Name.Should().Be(countryName);
        exception.Which.InnerException.Should().BeOfType<DbUpdateException>();

        await using var assertContext = Fixture.CreateContext();

        var countriesCount = await assertContext.Countries.CountAsync(x => x.Name == countryName, CancellationToken);
        countriesCount.Should().Be(1);
    }
}