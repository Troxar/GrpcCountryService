namespace CountryService.DAL.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly CountryContext _countryContext;

    public CountryRepository(CountryContext countryContext)
    {
        _countryContext = countryContext;
    }

    public async Task<int> CreateAsync(CreateCountryModel countryToCreate,
        CancellationToken cancellationToken = default)
    {
        var country = countryToCreate.ToEntity();

        await _countryContext.Countries.AddAsync(country, cancellationToken);
        await _countryContext.SaveChangesAsync(cancellationToken);

        return country.Id;
    }

    public async Task<int> UpdateAsync(UpdateCountryModel countryToUpdate,
        CancellationToken cancellationToken = default)
    {
        var country = await _countryContext.Countries
            .FirstOrDefaultAsync(x => x.Id == countryToUpdate.Id, cancellationToken);

        if (country is null)
            return 0;

        country.Description = countryToUpdate.Description;
        country.UpdateDate = countryToUpdate.UpdateDate;

        return await _countryContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _countryContext.Countries
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _countryContext.Countries
            .AsNoTracking()
            .ToModel()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _countryContext.Countries
            .AsNoTracking()
            .ToModel()
            .ToArrayAsync(cancellationToken);
    }
}