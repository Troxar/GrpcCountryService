namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public class GetAllTests : CountryRepositoryTestsBase
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

        Client.GetAll(Arg.Any<Empty>(),
                Arg.Any<Metadata?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateServerStreamingCall(replies));

        // Act
        var result = await Repository.GetAllAsync().ToArrayAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().HaveCount(replies.Length);
        foreach (var reply in replies)
            result.Should().ContainSingle(x =>
                x.Id == reply.Id
                && x.Name == reply.Name
                && x.Description == reply.Description);
    }
}