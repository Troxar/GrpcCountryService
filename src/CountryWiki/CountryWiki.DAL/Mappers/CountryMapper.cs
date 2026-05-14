namespace CountryWiki.DAL.Mappers;

public static class CountryMapper
{
    public static CreatedCountryModel ToModel(this CountryCreateReply reply)
    {
        return new CreatedCountryModel
        {
            Id = reply.Id,
            Name = reply.Name
        };
    }

    public static CountryModel? ToModel(this CountryReply? reply)
    {
        if (reply is null)
            return null;

        return new CountryModel
        {
            Id = reply.Id,
            Name = reply.Name,
            Description = reply.Description,
            FlagUri = reply.FlagUri,
            Anthem = reply.Anthem,
            CapitalCity = reply.CapitalCity,
            Languages = reply.Languages
        };
    }

    public static CountryCreateRequest ToRequest(this CreateCountryModel model)
    {
        var request = new CountryCreateRequest
        {
            Name = model.Name,
            Description = model.Description,
            Anthem = model.Anthem,
            CapitalCity = model.CapitalCity,
            FlagUri = model.FlagUri
        };
        request.Languages.AddRange(model.Languages);
        return request;
    }

    public static CountryUpdateRequest ToRequest(this UpdateCountryModel model)
    {
        return new CountryUpdateRequest
        {
            Id = model.Id,
            Description = model.Description
        };
    }
}