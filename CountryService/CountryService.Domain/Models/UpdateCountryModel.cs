namespace CountryService.Domain.Models;

public record UpdateCountryModel
{
    public int Id { get; init; }
    public required string Description { get; init; }
    public DateTime UpdateDate { get; init; }
}