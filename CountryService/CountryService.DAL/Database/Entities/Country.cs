namespace CountryService.DAL.Database.Entities;

public class Country
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string CapitalCity { get; set; }
    public required string Anthem { get; set; }
    public required string FlagUri { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public ICollection<CountryLanguage>? CountryLanguages { get; set; }
}