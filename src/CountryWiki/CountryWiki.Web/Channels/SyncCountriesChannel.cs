namespace CountryWiki.Web.Channels;

public class SyncCountriesChannel : ISyncCountriesChannel
{
    private readonly Channel<IEnumerable<CreateCountryModel>> _channel;
    private readonly ILogger<SyncCountriesChannel> _logger;

    public SyncCountriesChannel(ILogger<SyncCountriesChannel> logger)
    {
        var options = new UnboundedChannelOptions
        {
            SingleWriter = false,
            SingleReader = true
        };
        _channel = Channel.CreateUnbounded<IEnumerable<CreateCountryModel>>(options);
        _logger = logger;
    }

    public IAsyncEnumerable<IEnumerable<CreateCountryModel>> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }

    public async Task<bool> SyncAsync(IEnumerable<CreateCountryModel> countriesToCreate,
        CancellationToken cancellationToken)
    {
        var models = countriesToCreate.ToArray();
        while (await _channel.Writer.WaitToWriteAsync(cancellationToken) 
               && !cancellationToken.IsCancellationRequested)
        {
            if (!_channel.Writer.TryWrite(models))
                continue;

            _logger.LogDebug("Sending parsed countries to the background task");
            return true;
        }

        return false;
    }
}