namespace CountryService.Grpc.IntegrationTests.Infrastructure;

internal static class TestDataFactory
{
    internal static Country CreateCountry()
    {
        return new Country
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            CapitalCity = Guid.NewGuid().ToString(),
            Anthem = Guid.NewGuid().ToString(),
            FlagUri = Guid.NewGuid().ToString(),
            CreateDate = DateTime.UtcNow
        };
    }

    internal static CountryCreateRequest CreateCountryCreateRequest()
    {
        return new CountryCreateRequest
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            CapitalCity = Guid.NewGuid().ToString(),
            Anthem = Guid.NewGuid().ToString(),
            FlagUri = Guid.NewGuid().ToString()
        };
    }

    internal static CountryIdRequest CreateCountryIdRequest(int id)
    {
        return new CountryIdRequest
        {
            Id = id
        };
    }

    internal static CountryUpdateRequest CreateCountryUpdateRequest(int id)
    {
        return new CountryUpdateRequest
        {
            Id = id,
            Description = Guid.NewGuid().ToString()
        };
    }
}