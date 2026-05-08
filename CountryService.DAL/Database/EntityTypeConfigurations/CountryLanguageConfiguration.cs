namespace CountryService.DAL.Database.EntityTypeConfigurations;

public class CountryLanguageConfiguration : IEntityTypeConfiguration<CountryLanguage>
{
    public void Configure(EntityTypeBuilder<CountryLanguage> builder)
    {
        builder.HasKey(x => new { x.CountryId, x.LanguageId });
        builder.HasOne(x => x.Country)
            .WithMany(x => x.CountryLanguages)
            .HasForeignKey(x => x.CountryId);
        builder.HasOne(x => x.Language)
            .WithMany(x => x.CountryLanguages)
            .HasForeignKey(x => x.LanguageId);
    }
}