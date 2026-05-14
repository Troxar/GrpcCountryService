namespace CountryWiki.Domain.Models;

public record CreatedCountryModel
{
    public int Id { get; init; }
    public required string Name { get; init; }
}