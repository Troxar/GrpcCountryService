namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public class CreateTests : CountryRepositoryTestsBase
{
    [Fact]
    public async Task ShouldWriteRequests_CompleteStream_AndReturnReplies_WhenCountriesProvided()
    {
        // Arrange
        var countriesToCreate = new[]
        {
            TestDataFactory.CreateCreateCountryModel(),
            TestDataFactory.CreateCreateCountryModel()
        };
        var requestStream = new TestClientStreamWriter<CountryCreateRequest>();
        var replies = countriesToCreate.Select((x, i) => TestDataFactory.CreateCountryCreateReply(i, x.Name))
            .ToArray();

        Client.Create(Arg.Any<Metadata?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateDuplexStreamingCall(requestStream, replies));


        // Act
        var result = await Repository.CreateAsync(countriesToCreate)
            .ToArrayAsync(TestContext.Current.CancellationToken);

        // Assert
        requestStream.IsCompleted.Should().BeTrue();
        requestStream.Messages.Should().HaveCount(countriesToCreate.Length);
        foreach (var countryToCreate in countriesToCreate)
            requestStream.Messages.Should().ContainSingle(x =>
                x.Name == countryToCreate.Name
                && x.Description == countryToCreate.Description
                && x.FlagUri == countryToCreate.FlagUri
                && x.Anthem == countryToCreate.Anthem
                && x.CapitalCity == countryToCreate.CapitalCity);

        result.Should().HaveCount(replies.Length);
        foreach (var reply in replies)
            result.Should().ContainSingle(x =>
                x.Id == reply.Id
                && x.Name == reply.Name);
    }
}