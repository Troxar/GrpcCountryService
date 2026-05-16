namespace CountryWiki.BLL.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;
    private readonly ILogger<CountryService> _logger;

    public CountryService(ICountryRepository countryRepository, ILogger<CountryService> logger)
    {
        _countryRepository = countryRepository;
        _logger = logger;
    }

    public async Task CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate)
    {
        await foreach (var createdCountry in _countryRepository.CreateAsync(countriesToCreate))
            _logger.LogDebug("Country {CreatedCountryName} has been created with Id {CreatedCountryId}",
                createdCountry.Name, createdCountry.Id);
    }

    public async Task UpdateAsync(UpdateCountryModel countryToUpdate)
    {
        await _countryRepository.UpdateAsync(countryToUpdate);
        _logger.LogDebug("Country with Id {UpdatedCountryId} has been successfully updated", countryToUpdate.Id);
    }

    public async Task DeleteAsync(int id)
    {
        await _countryRepository.DeleteAsync(id);
        _logger.LogDebug("Country with Id {DeletedCountryId} has been successfully deleted", id);
    }

    public async Task<CountryModel?> GetAsync(int id)
    {
        return await _countryRepository.GetAsync(id);
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync()
    {
        return await _countryRepository.GetAllAsync().ToListAsync();
    }
}