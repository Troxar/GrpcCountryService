namespace CountryWiki.BLL.Tests.Infrastructure;

internal static class TestDataFactory
{
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

    internal static UpdateCountryModel CreateUpdateCountryModel(int id)
    {
        return new UpdateCountryModel
        {
            Id = id,
            Description = Guid.NewGuid().ToString()
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

    internal static RpcException CreateRpcException(StatusCode statusCode, string? detail = null)
    {
        return new RpcException(new Status(statusCode, detail ?? Guid.NewGuid().ToString()));
    }
}