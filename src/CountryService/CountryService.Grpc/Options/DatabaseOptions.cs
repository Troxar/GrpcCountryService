namespace CountryService.Grpc.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";
    public bool ApplyMigrationsOnStartup { get; init; }
}