namespace CountryService.DAL.Tests.Infrastructure;

internal static class TestDataFactory
{
    internal static CreateCountryModel CreateCountryModel(string? name = null, params int[] languageIds)
    {
        return new CreateCountryModel
        {
            Name = name ?? Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            FlagUri = Guid.NewGuid().ToString(),
            CapitalCity = Guid.NewGuid().ToString(),
            Anthem = Guid.NewGuid().ToString(),
            Languages = languageIds
        };
    }

    internal static Country CreateCountry(string? name = null)
    {
        return new Country
        {
            Name = name ?? Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            FlagUri = Guid.NewGuid().ToString(),
            CapitalCity = Guid.NewGuid().ToString(),
            Anthem = Guid.NewGuid().ToString(),
            CreateDate = DateTime.UtcNow
        };
    }

    internal static UpdateCountryModel UpdateCountryModel(int countryId)
    {
        var model = new UpdateCountryModel
        {
            Id = countryId,
            Description = Guid.NewGuid().ToString(),
            UpdateDate = DateTime.UtcNow
        };
        return model;
    }
}