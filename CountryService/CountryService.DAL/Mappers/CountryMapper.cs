namespace CountryService.DAL.Mappers;

public static class CountryMapper
{
    public static IQueryable<CountryModel> ToModel(this IQueryable<Country> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static Country ToEntity(this CreateCountryModel countryToCreate)
    {
        return new Country
        {
            Name = countryToCreate.Name,
            Description = countryToCreate.Description,
            CapitalCity = countryToCreate.CapitalCity,
            Anthem = countryToCreate.Anthem,
            FlagUri = countryToCreate.FlagUri,
            CreateDate = DateTime.UtcNow,
            CountryLanguages = countryToCreate.Languages
                .Select(x => new CountryLanguage { LanguageId = x })
                .ToArray()
        };
    }

    private static CountryModel ToModel(this Country entity)
    {
        return new CountryModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            CapitalCity = entity.CapitalCity,
            Anthem = entity.Anthem,
            FlagUri = entity.FlagUri,
            Languages = entity.CountryLanguages!.Select(y => y.Language!.Name)
        };
    }
}