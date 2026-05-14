namespace CountryWiki.DAL.Repositories;

using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

public class CountryRepository : ICountryRepository
{
    private readonly CountryServiceClient _countryServiceClient;

    public CountryRepository(CountryServiceClient countryServiceClient)
    {
        _countryServiceClient = countryServiceClient;
    }

    public async IAsyncEnumerable<CreatedCountryModel> CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate)
    {
        using var streamingCall = _countryServiceClient.Create();
        foreach (var countryToCreate in countriesToCreate)
        {
            var request = countryToCreate.ToRequest();
            await streamingCall.RequestStream.WriteAsync(request);
        }

        await streamingCall.RequestStream.CompleteAsync();

        while (await streamingCall.ResponseStream.MoveNext(CancellationToken.None))
        {
            var reply = streamingCall.ResponseStream.Current;
            yield return reply.ToModel();
        }
    }

    public async Task UpdateAsync(UpdateCountryModel countryToUpdate)
    {
        var request = countryToUpdate.ToRequest();
        await _countryServiceClient.UpdateAsync(request);
    }

    public async Task DeleteAsync(int id)
    {
        var request = new CountryIdRequest { Id = id };
        await _countryServiceClient.DeleteAsync(request);
    }

    public async Task<CountryModel?> GetAsync(int id)
    {
        var request = new CountryIdRequest { Id = id };
        var reply = await _countryServiceClient.GetAsync(request);
        return reply.ToModel();
    }

    public async IAsyncEnumerable<CountryModel> GetAllAsync()
    {
        var request = new Empty();
        using var streamingCall = _countryServiceClient.GetAll(request);
        while (await streamingCall.ResponseStream.MoveNext(CancellationToken.None))
            yield return streamingCall.ResponseStream.Current.ToModel()!;
    }
}