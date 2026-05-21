namespace CountryWiki.Web.IntegrationTests.Infrastructure;

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
}