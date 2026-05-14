namespace CountryWiki.Domain.Repositories;

public interface ICountryRepository
{
    IAsyncEnumerable<CreatedCountryModel> CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate);
    Task UpdateAsync(UpdateCountryModel countryToUpdate);
    Task DeleteAsync(int id);
    Task<CountryModel?> GetAsync(int id);
    IAsyncEnumerable<CountryModel> GetAllAsync();
}