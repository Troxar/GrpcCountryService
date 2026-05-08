namespace CountryService.DAL.Database.Entities;

public class Language
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<CountryLanguage>? CountryLanguages { get; set; }
}