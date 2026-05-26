namespace CountryService.BLL.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<int> CreateAsync(CreateCountryModel countryToCreate,
        CancellationToken cancellationToken = default)
    {
        return await _countryRepository.CreateAsync(countryToCreate, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateCountryModel countryToUpdate,
        CancellationToken cancellationToken = default)
    {
        return await _countryRepository.UpdateAsync(countryToUpdate, cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _countryRepository.DeleteAsync(id, cancellationToken) > 0;
    }

    public async Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _countryRepository.GetAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _countryRepository.GetAllAsync(cancellationToken);
    }
}