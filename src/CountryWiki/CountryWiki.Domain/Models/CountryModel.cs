namespace CountryWiki.Domain.Models;

public record CountryModel
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string FlagUri { get; init; }
    public required string CapitalCity { get; init; }
    public required string Anthem { get; init; }
    public required IEnumerable<string> Languages { get; init; }
}