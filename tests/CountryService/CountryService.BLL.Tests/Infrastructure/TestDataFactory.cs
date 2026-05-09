namespace CountryService.BLL.Tests.Infrastructure;

internal static class TestDataFactory
{
    internal static UpdateCountryModel CreateUpdateCountryModel(int id)
    {
        return new UpdateCountryModel
        {
            Id = id,
            Description = Guid.NewGuid().ToString(),
            UpdateDate = DateTime.UtcNow
        };
    }
}