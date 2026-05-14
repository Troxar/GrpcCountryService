namespace CountryWiki.Domain.Models;

public record UpdateCountryModel
{
    public int Id { get; init; }
    public required string Description { get; init; }
}