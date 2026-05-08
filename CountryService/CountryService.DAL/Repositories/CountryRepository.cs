namespace CountryService.DAL.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly CountryContext _countryContext;

    public CountryRepository(CountryContext countryContext)
    {
        _countryContext = countryContext;
    }

    public async Task<int> CreateAsync(CreateCountryModel countryToCreate)
    {
        var country = countryToCreate.ToEntity();

        await _countryContext.Countries.AddAsync(country);
        await _countryContext.SaveChangesAsync();

        return country.Id;
    }

    public async Task<int> UpdateAsync(UpdateCountryModel countryToUpdate)
    {
        var country = await _countryContext.Countries
            .FirstOrDefaultAsync(x => x.Id == countryToUpdate.Id);

        if (country is null)
            return 0;

        country.Description = countryToUpdate.Description;
        country.UpdateDate = countryToUpdate.UpdateDate;

        return await _countryContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _countryContext.Countries
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<CountryModel?> GetAsync(int id)
    {
        return await _countryContext.Countries
            .AsNoTracking()
            .ToModel()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync()
    {
        return await _countryContext.Countries
            .AsNoTracking()
            .ToModel()
            .ToArrayAsync();
    }
}