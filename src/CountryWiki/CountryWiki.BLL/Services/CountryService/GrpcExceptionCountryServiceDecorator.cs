namespace CountryWiki.BLL.Services.CountryService;

public sealed class GrpcExceptionCountryServiceDecorator : ICountryService
{
    private readonly ICountryService _inner;

    public GrpcExceptionCountryServiceDecorator(ICountryService inner)
    {
        _inner = inner;
    }

    public async Task CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate,
        CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(() => _inner.CreateAsync(countriesToCreate, cancellationToken));
    }

    public async Task UpdateAsync(UpdateCountryModel countryToUpdate, CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(() => _inner.UpdateAsync(countryToUpdate, cancellationToken));
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(() => _inner.DeleteAsync(id, cancellationToken));
    }

    public async Task<CountryModel?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _inner.GetAsync(id, cancellationToken);
        }
        catch (RpcException exception) when (exception.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
        catch (RpcException exception)
        {
            throw exception.ToCountryServiceException();
        }
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => _inner.GetAllAsync(cancellationToken));
    }

    private static async Task ExecuteAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (RpcException exception)
        {
            throw exception.ToCountryServiceException();
        }
    }

    private static async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (RpcException exception)
        {
            throw exception.ToCountryServiceException();
        }
    }
}