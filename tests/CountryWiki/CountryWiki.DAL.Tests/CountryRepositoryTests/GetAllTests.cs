namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public sealed class GetAllTests : CountryRepositoryTestsBase
{
    [Fact]
    public async Task ShouldReturnMappedCountries_WhenCountriesExist()
    {
        // Arrange
        var replies = new[]
        {
            TestDataFactory.CreateCountryReply(1),
            TestDataFactory.CreateCountryReply(2)
        };
        Client.GetAll(Arg.Any<Empty>(), Arg.Any<Metadata?>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateServerStreamingCall(replies));

        // Act
        var result = await Repository.GetAllAsync(CancellationToken).ToArrayAsync(CancellationToken);

        // Assert
        result.Should().BeEquivalentTo(replies, options => options.ComparingByMembers<CountryReply>());
    }
}