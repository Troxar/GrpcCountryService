namespace CountryWiki.Web.Tests.Infrastructure;

internal static class TestDataFactory
{
    internal static IFormFile CreateFormFile(string fileName, string contentType, string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        return new FormFile(stream, 0, bytes.Length, "Upload", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
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

    internal static UpdateCountry CreateUpdateCountry(int id)
    {
        return new UpdateCountry
        {
            Id = id,
            Description = Guid.NewGuid().ToString()
        };
    }

    internal static CountryServiceException CreateCountryServiceException(CountryServiceErrorCode errorCode,
        string? message = null)
    {
        return new CountryServiceException(errorCode, message ?? Guid.NewGuid().ToString());
    }
}