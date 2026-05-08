namespace CountryService.BLL.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<int> CreateAsync(CreateCountryModel countryToCreate)
    {
        return await _countryRepository.CreateAsync(countryToCreate);
    }

    public async Task<bool> UpdateAsync(UpdateCountryModel countryToUpdate)
    {
        return await _countryRepository.UpdateAsync(countryToUpdate) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _countryRepository.DeleteAsync(id) > 0;
    }

    public async Task<CountryModel?> GetAsync(int id)
    {
        return await _countryRepository.GetAsync(id);
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync()
    {
        return await _countryRepository.GetAllAsync();
    }
}