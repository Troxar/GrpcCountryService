namespace CountryService.Grpc.Services;

public class CountryGrpcService : v1.CountryService.CountryServiceBase
{
    private readonly ICountryService _countryService;
    private readonly IValidator<CountryCreateRequest> _createRequestValidator;
    private readonly IValidator<CountryUpdateRequest> _updateRequestValidator;
    private readonly IValidator<CountryIdRequest> _idRequestValidator;

    public CountryGrpcService(ICountryService countryService, IValidator<CountryCreateRequest> createRequestValidator,
        IValidator<CountryUpdateRequest> updateRequestValidator, IValidator<CountryIdRequest> idRequestValidator)
    {
        _countryService = countryService;
        _createRequestValidator = createRequestValidator;
        _updateRequestValidator = updateRequestValidator;
        _idRequestValidator = idRequestValidator;
    }

    public override async Task Create(IAsyncStreamReader<CountryCreateRequest> requestStream,
        IServerStreamWriter<CountryCreateReply> responseStream, ServerCallContext context)
    {
        await foreach (var countryToCreate in requestStream.ReadAllAsync(context.CancellationToken))
        {
            await _createRequestValidator.ValidateAndThrowRpcExceptionsAsync(countryToCreate,
                context.CancellationToken);

            var model = countryToCreate.ToModel();
            var id = await _countryService.CreateAsync(model, context.CancellationToken);

            context.CancellationToken.ThrowIfCancellationRequested();

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
        await _updateRequestValidator.ValidateAndThrowRpcExceptionsAsync(request, context.CancellationToken);

        var model = request.ToModel();
        var updateSucceed = await _countryService.UpdateAsync(model, context.CancellationToken);
        if (!updateSucceed)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Country with Id {request.Id} hasn't been updated. It has probably been deleted."));

        return new Empty();
    }

    public override async Task<Empty> Delete(CountryIdRequest request, ServerCallContext context)
    {
        await _idRequestValidator.ValidateAndThrowRpcExceptionsAsync(request, context.CancellationToken);

        var deleteSucceed = await _countryService.DeleteAsync(request.Id, context.CancellationToken);
        if (!deleteSucceed)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Country with Id {request.Id} hasn't been deleted. It has probably already been deleted."));

        return new Empty();
    }

    public override async Task<CountryReply> Get(CountryIdRequest request, ServerCallContext context)
    {
        await _idRequestValidator.ValidateAndThrowRpcExceptionsAsync(request, context.CancellationToken);

        var model = await _countryService.GetAsync(request.Id, context.CancellationToken);
        if (model is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Country with Id {request.Id} hasn't been found"));

        return model.ToReply();
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<CountryReply> responseStream,
        ServerCallContext context)
    {
        foreach (var model in await _countryService.GetAllAsync(context.CancellationToken))
        {
            var reply = model.ToReply();
            await responseStream.WriteAsync(reply);
        }
    }
}