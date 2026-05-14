namespace CountryWiki.Domain.Models;

public record CreateCountryModel
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string FlagUri { get; init; }
    public required string CapitalCity { get; init; }
    public required string Anthem { get; init; }
    public required IEnumerable<int> Languages { get; init; }
}