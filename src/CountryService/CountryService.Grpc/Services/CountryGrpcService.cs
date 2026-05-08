namespace CountryService.Grpc.Services;

public class CountryGrpcService : v1.CountryService.CountryServiceBase
{
    private readonly ICountryService _countryService;

    public CountryGrpcService(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public override async Task Create(IAsyncStreamReader<CountryCreateRequest> requestStream,
        IServerStreamWriter<CountryCreateReply> responseStream, ServerCallContext context)
    {
        await foreach (var countryToCreate in requestStream.ReadAllAsync())
        {
            var model = countryToCreate.ToModel();
            var id = await _countryService.CreateAsync(model);
            var reply = new CountryCreateReply
            {
                Id = id,
                Name = model.Name
            };
            await responseStream.WriteAsync(reply);
        }
    }

    public override async Task<Empty> Update(CountryUpdateRequest request, ServerCallContext context)
    {
        var model = request.ToModel();
        var updateSucceed = await _countryService.UpdateAsync(model);
        if (!updateSucceed)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Country with Id {request.Id} hasn't been updated. It has probably been deleted."));

        return new Empty();
    }

    public override async Task<Empty> Delete(CountryIdRequest request, ServerCallContext context)
    {
        var deleteSucceed = await _countryService.DeleteAsync(request.Id);
        if (!deleteSucceed)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Country with Id {request.Id} hasn't been deleted. It has probably already been deleted."));

        return new Empty();
    }

    public override async Task<CountryReply> Get(CountryIdRequest request, ServerCallContext context)
    {
        var model = await _countryService.GetAsync(request.Id);
        if (model is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Country with Id {request.Id} hasn't been found"));

        return model.ToReply();
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<CountryReply> responseStream,
        ServerCallContext context)
    {
        foreach (var model in await _countryService.GetAllAsync())
        {
            var reply = model.ToReply();
            await responseStream.WriteAsync(reply);
        }
    }
}