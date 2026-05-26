namespace CountryWiki.BLL.Services.CountryService;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;
    private readonly ILogger<CountryService> _logger;

    public CountryService(ICountryRepository countryRepository, ILogger<CountryService> logger)
    {
        _countryRepository = countryRepository;
        _logger = logger;
    }

    public async Task CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate,
        CancellationToken cancellationToken = default)
    {
        await foreach (var createdCountry in _countryRepository.CreateAsync(countriesToCreate, cancellationToken))
            _logger.LogDebug("Country {CreatedCountryName} has been created with Id {CreatedCountryId}",
                createdCountry.Name, createdCountry.Id);
    }

    public async Task UpdateAsync(UpdateCountryModel countryToUpdate, CancellationToken cancellationToken = default)
    {
        await _countryRepository.UpdateAsync(countryToUpdate, cancellationToken);
        _logger.LogDebug("Country with Id {UpdatedCountryId} has been successfully updated", countryToUpdate.Id);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _countryRepository.DeleteAsync(id, cancellationToken);
        _logger.LogDebug("Country with Id {DeletedCountryId} has been successfully deleted", id);
    }

    public async Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _countryRepository.GetAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _countryRepository.GetAllAsync(cancellationToken)
            .ToListAsync(cancellationToken);
    }
}