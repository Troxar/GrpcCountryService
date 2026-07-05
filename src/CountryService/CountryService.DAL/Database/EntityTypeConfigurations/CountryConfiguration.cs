namespace CountryService.DAL.Database.EntityTypeConfigurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name).IsUnique();
            
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.CapitalCity).HasMaxLength(50);
        builder.Property(x => x.Anthem).HasMaxLength(200);
        builder.Property(x => x.FlagUri).HasMaxLength(200);
        builder.Property(x => x.CreateDate).HasDefaultValueSql("now()").ValueGeneratedOnAdd();
    }
}