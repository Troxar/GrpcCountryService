namespace CountryWiki.Domain.Repositories;

public interface ICountryRepository
{
    IAsyncEnumerable<CreatedCountryModel> CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateCountryModel countryToUpdate, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default);
    IAsyncEnumerable<CountryModel> GetAllAsync(CancellationToken cancellationToken = default);
}