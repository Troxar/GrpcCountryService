namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public sealed class CreateTests : CountryRepositoryTestsBase
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
        var replies = countriesToCreate.Select((x, i) => TestDataFactory.CreateCountryCreateReply(i, x.Name)).ToArray();
        Client.Create(Arg.Any<Metadata?>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateDuplexStreamingCall(requestStream, replies));


        // Act
        var result = await Repository.CreateAsync(countriesToCreate, CancellationToken)
            .ToArrayAsync(CancellationToken);

        // Assert
        requestStream.IsCompleted.Should().BeTrue();
        requestStream.Messages.Should().BeEquivalentTo(countriesToCreate);

        result.Should().BeEquivalentTo(replies, options => options
            .ComparingByMembers<CountryCreateReply>());
    }
}