namespace CountryWiki.Web.Channels;

public interface ISyncCountriesChannel
{
    IAsyncEnumerable<IEnumerable<CreateCountryModel>> ReadAllAsync(CancellationToken cancellationToken);
    Task<bool> SyncAsync(IEnumerable<CreateCountryModel> countriesToCreate, CancellationToken cancellationToken);
}