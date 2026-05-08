namespace CountryService.DAL.Mappers;

public static class CountryMapper
{
    public static IQueryable<CountryModel> ToModel(this IQueryable<Country> entities)
    {
        return entities.Select(x => new CountryModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CapitalCity = x.CapitalCity,
            Anthem = x.Anthem,
            FlagUri = x.FlagUri,
            Languages = x.CountryLanguages!.Select(y => y.Language!.Name)
        });
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
}