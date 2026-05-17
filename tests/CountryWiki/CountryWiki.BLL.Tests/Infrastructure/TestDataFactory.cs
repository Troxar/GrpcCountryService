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
}