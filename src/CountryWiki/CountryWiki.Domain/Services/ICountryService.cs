namespace CountryWiki.Domain.Services;

public interface ICountryService
{
    Task CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateCountryModel countryToUpdate, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CountryModel>> GetAllAsync(CancellationToken cancellationToken = default);
}