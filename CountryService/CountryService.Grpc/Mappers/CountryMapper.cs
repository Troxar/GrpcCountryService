using CountryService.Domain.Models;

namespace CountryService.Grpc.Mappers;

public static class CountryMapper
{
    public static CreateCountryModel ToModel(this CountryCreateRequest request)
    {
        return new CreateCountryModel
        {
            Name = request.Name,
            Description = request.Description,
            Anthem = request.Anthem,
            CapitalCity = request.CapitalCity,
            FlagUri = request.FlagUri,
            Languages = request.Languages
        };
    }

    public static UpdateCountryModel ToModel(this CountryUpdateRequest request)
    {
        return new UpdateCountryModel
        {
            Id = request.Id,
            Description = request.Description,
            UpdateDate = DateTime.UtcNow
        };
    }
    
    public static CountryReply ToReply(this CountryModel model)
    {
        var reply = new CountryReply
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Anthem = model.Anthem,
            CapitalCity = model.CapitalCity,
            FlagUri = model.FlagUri
        };
        reply.Languages.AddRange(model.Languages);
        return reply;
    }
}