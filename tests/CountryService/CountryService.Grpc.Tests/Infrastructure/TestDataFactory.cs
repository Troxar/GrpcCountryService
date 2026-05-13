namespace CountryService.Grpc.Tests.Infrastructure;

internal static class TestDataFactory
{
    internal static ServerCallContext CreateServerCallContext(string? correlationId = null)
    {
        var requestHeaders = new Metadata();
        if (correlationId is not null)
            requestHeaders.Add("x-correlation-id", correlationId);
        
        return TestServerCallContext.Create(
            method: "test",
            host: "localhost",
            deadline: DateTime.UtcNow.AddMinutes(1),
            requestHeaders: requestHeaders,
            cancellationToken: CancellationToken.None,
            peer: "ipv4:127.0.0.1:5000",
            authContext: null,
            contextPropagationToken: null,
            writeHeadersFunc: _ => Task.CompletedTask,
            writeOptionsGetter: () => null,
            writeOptionsSetter: _ => { });
    }

    internal static TestServerStreamWriter<T> CreateServerStreamWriter<T>()
    {
        return new TestServerStreamWriter<T>();
    }

    internal static TestAsyncStreamReader<T> CreateAsyncStreamReader<T>(IEnumerable<T> items)
    {
        return new TestAsyncStreamReader<T>(items);
    }

    internal static CountryUpdateRequest CreateCountryUpdateRequest(int id)
    {
        return new CountryUpdateRequest
        {
            Id = id,
            Description = Guid.NewGuid().ToString()
        };
    }

    internal static CountryIdRequest CreateCountryIdRequest(int id)
    {
        return new CountryIdRequest
        {
            Id = id
        };
    }

    internal static CountryModel CreateCountryModel(int id)
    {
        return new CountryModel
        {
            Id = id,
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            FlagUri = Guid.NewGuid().ToString(),
            CapitalCity = Guid.NewGuid().ToString(),
            Anthem = Guid.NewGuid().ToString(),
            Languages = []
        };
    }

    internal static CountryCreateRequest CreateCountryCreateRequest()
    {
        return new CountryCreateRequest
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString()
        };
    }
}