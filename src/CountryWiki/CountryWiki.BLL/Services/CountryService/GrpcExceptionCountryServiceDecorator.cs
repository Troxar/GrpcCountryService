namespace CountryWiki.BLL.Services.CountryService;

public sealed class GrpcExceptionCountryServiceDecorator : ICountryService
{
    private readonly ICountryService _inner;

    public GrpcExceptionCountryServiceDecorator(ICountryService inner)
    {
        _inner = inner;
    }

    public async Task CreateAsync(IEnumerable<CreateCountryModel> countriesToCreate)
    {
        await ExecuteAsync(() => _inner.CreateAsync(countriesToCreate));
    }

    public async Task UpdateAsync(UpdateCountryModel countryToUpdate)
    {
        await ExecuteAsync(() => _inner.UpdateAsync(countryToUpdate));
    }

    public async Task DeleteAsync(int id)
    {
        await ExecuteAsync(() => _inner.DeleteAsync(id));
    }

    public async Task<CountryModel?> GetAsync(int id)
    {
        try
        {
            return await _inner.GetAsync(id);
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

    public async Task<IEnumerable<CountryModel>> GetAllAsync()
    {
        return await ExecuteAsync(() => _inner.GetAllAsync());
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