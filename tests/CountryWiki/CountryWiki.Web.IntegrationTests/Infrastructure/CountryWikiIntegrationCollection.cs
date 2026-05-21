namespace CountryWiki.Web.IntegrationTests.Infrastructure;

[CollectionDefinition(Name, DisableParallelization = true)]
public class CountryWikiIntegrationCollection : ICollectionFixture<CountryWikiIntegrationFixture>
{
    public const string Name = "CountryWiki integration collection";
}