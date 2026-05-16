namespace CountryWiki.DAL.Tests.Infrastructure;

internal static class TestDataFactory
{
    internal static AsyncUnaryCall<TResponse> CreateUnaryCall<TResponse>(TResponse response)
    {
        return new AsyncUnaryCall<TResponse>(
            responseAsync: Task.FromResult(response),
            responseHeadersAsync: Task.FromResult(new Metadata()),
            getStatusFunc: () => Status.DefaultSuccess,
            getTrailersFunc: () => [],
            disposeAction: () => { });
    }

    internal static AsyncUnaryCall<TResponse> CreateFailedUnaryCall<TResponse>(Exception exception)
    {
        return new AsyncUnaryCall<TResponse>(
            responseAsync: Task.FromException<TResponse>(exception),
            responseHeadersAsync: Task.FromResult(new Metadata()),
            getStatusFunc: () => new Status(StatusCode.Unknown, exception.Message),
            getTrailersFunc: () => [],
            disposeAction: () => { });
    }

    internal static AsyncServerStreamingCall<TResponse> CreateServerStreamingCall<TResponse>(
        IEnumerable<TResponse> responses)
    {
        return new AsyncServerStreamingCall<TResponse>(
            responseStream: new TestAsyncStreamReader<TResponse>(responses),
            responseHeadersAsync: Task.FromResult(new Metadata()),
            getStatusFunc: () => Status.DefaultSuccess,
            getTrailersFunc: () => [],
            disposeAction: () => { });
    }

    internal static AsyncDuplexStreamingCall<TRequest, TResponse> CreateDuplexStreamingCall<TRequest, TResponse>(
        IClientStreamWriter<TRequest> requestStream, IEnumerable<TResponse> responses)
    {
        return new AsyncDuplexStreamingCall<TRequest, TResponse>(
            requestStream: requestStream,
            responseStream: new TestAsyncStreamReader<TResponse>(responses),
            responseHeadersAsync: Task.FromResult(new Metadata()),
            getStatusFunc: () => Status.DefaultSuccess,
            getTrailersFunc: () => [],
            disposeAction: () => { });
    }

    internal static UpdateCountryModel CreateUpdateCountryModel(int id)
    {
        return new UpdateCountryModel
        {
            Id = id,
            Description = Guid.NewGuid().ToString()
        };
    }

    internal static CountryReply CreateCountryReply(int id)
    {
        return new CountryReply
        {
            Id = id,
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            FlagUri = Guid.NewGuid().ToString(),
            Anthem = Guid.NewGuid().ToString(),
            CapitalCity = Guid.NewGuid().ToString()
        };
    }
    
    internal static CountryCreateReply CreateCountryCreateReply(int id, string name)
    {
        return new CountryCreateReply
        {
            Id = id,
            Name = name
        };
    }

    internal static CreateCountryModel CreateCreateCountryModel()
    {
        return new CreateCountryModel
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            FlagUri = Guid.NewGuid().ToString(),
            Anthem = Guid.NewGuid().ToString(),
            CapitalCity = Guid.NewGuid().ToString(),
            Languages = []
        };
    }
}