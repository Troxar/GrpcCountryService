namespace CountryWiki.DAL.Repositories;

using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

public class CountryRepository : ICountryRepository
{
    private readonly CountryServiceClient _countryServiceClient;
    private readonly GrpcOptions _grpcOptions;

    public CountryRepository(CountryServiceClient countryServiceClient, IOptions<GrpcOptions> grpcOptions)
    {
        _countryServiceClient = countryServiceClient;
        _grpcOptions = grpcOptions.Value;
    }

    public async IAsyncEnumerable<CreatedCountryModel> CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var streamingCall =
            _countryServiceClient.Create(deadline: CreateDeadline(), cancellationToken: cancellationToken);

        foreach (var countryToCreate in countriesToCreate)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var request = countryToCreate.ToRequest();
            // ReSharper disable once MethodSupportsCancellation
            await streamingCall.RequestStream.WriteAsync(request);
        }

        await streamingCall.RequestStream.CompleteAsync();

        while (await streamingCall.ResponseStream.MoveNext(cancellationToken))
        {
            var reply = streamingCall.ResponseStream.Current;
            yield return reply.ToModel();
        }
    }

    public async Task UpdateAsync(UpdateCountryModel countryToUpdate, CancellationToken cancellationToken = default)
    {
        var request = countryToUpdate.ToRequest();
        await _countryServiceClient.UpdateAsync(request, deadline: CreateDeadline(),
            cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = new CountryIdRequest { Id = id };
        await _countryServiceClient.DeleteAsync(request, deadline: CreateDeadline(),
            cancellationToken: cancellationToken);
    }

    public async Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = new CountryIdRequest { Id = id };
        var reply = await _countryServiceClient.GetAsync(request, deadline: CreateDeadline(),
            cancellationToken: cancellationToken);
        return reply.ToModel();
    }

    public async IAsyncEnumerable<CountryModel> GetAllAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new Empty();
        using var streamingCall =
            _countryServiceClient.GetAll(request, deadline: CreateDeadline(), cancellationToken: cancellationToken);
        while (await streamingCall.ResponseStream.MoveNext(cancellationToken))
            yield return streamingCall.ResponseStream.Current.ToModel()!;
    }

    private DateTime CreateDeadline()
    {
        return DateTime.UtcNow.Add(_grpcOptions.DefaultCallTimeout);
    }
}