namespace CountryWiki.Web.Background;

public class SyncUploadedCountriesBackgroundService : BackgroundService
{
    private readonly ILogger<SyncUploadedCountriesBackgroundService> _logger;
    private readonly GlobalOptions _globalOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISyncCountriesChannel _syncCountriesChannel;

    public SyncUploadedCountriesBackgroundService(ILogger<SyncUploadedCountriesBackgroundService> logger,
        GlobalOptions globalOptions, IServiceProvider serviceProvider, ISyncCountriesChannel syncCountriesChannel)
    {
        _logger = logger;
        _globalOptions = globalOptions;
        _serviceProvider = serviceProvider;
        _syncCountriesChannel = syncCountriesChannel;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var countries in _syncCountriesChannel.ReadAllAsync(cancellationToken))
            try
            {
                _logger.LogInformation("Received uploaded countries from the channel for sync");

                using var scope = _serviceProvider.CreateScope();
                var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();

                try
                {
                    _globalOptions.ProcessingUpload = true;
                    await countryService.CreateAsync(countries);
                }
                catch (RpcException ex)
                {
                    var correlationId = ex.Trailers.GetValue("correlationId");
                    _logger.LogError(ex, "Background synchronization has failed. CorrelationId: {correlationId}",
                        correlationId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to manage uploaded countries");
            }
            finally
            {
                _globalOptions.ProcessingUpload = false;
            }
    }
}