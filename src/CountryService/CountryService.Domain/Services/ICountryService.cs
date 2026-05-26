namespace CountryService.Domain.Services;

public interface ICountryService
{
    Task<int> CreateAsync(CreateCountryModel countryToCreate, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(UpdateCountryModel countryToUpdate, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CountryModel>> GetAllAsync(CancellationToken cancellationToken = default);
}