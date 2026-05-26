namespace CountryService.Domain.Repositories;

public interface ICountryRepository
{
    Task<int> CreateAsync(CreateCountryModel countryToCreate, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(UpdateCountryModel countryToUpdate, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CountryModel>> GetAllAsync(CancellationToken cancellationToken = default);
}