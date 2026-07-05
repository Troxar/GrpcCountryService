namespace CountryService.DAL.Tests.Infrastructure;

internal sealed class InsertDuplicateCountryBeforeSaveInterceptor : SaveChangesInterceptor
{
    private readonly DbContextOptions<CountryContext> _options;
    private readonly string _countryName;
    private bool _duplicateInserted;

    public InsertDuplicateCountryBeforeSaveInterceptor(DbContextOptions<CountryContext> options, string countryName)
    {
        _options = options;
        _countryName = countryName;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (_duplicateInserted)
            return result;

        _duplicateInserted = true;

        await using var context = new CountryContext(_options);

        var country = TestDataFactory.CreateCountry(_countryName);

        await context.Countries.AddAsync(country, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return result;
    }
}