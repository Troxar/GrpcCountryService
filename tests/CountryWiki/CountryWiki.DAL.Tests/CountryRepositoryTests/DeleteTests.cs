namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public class DeleteTests : CountryRepositoryTestsBase
{
    [Fact]
    public async Task ShouldSendCountryIdRequest()
    {
        // Arrange
        const int id = 1;
        Client.DeleteAsync(Arg.Any<CountryIdRequest>(),
                Arg.Any<Metadata?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateUnaryCall(new Empty()));

        // Act
        await Repository.DeleteAsync(id);

        // Assert
        _ = Client.Received(1).DeleteAsync(
            Arg.Is<CountryIdRequest>(x => x.Id == id),
            Arg.Any<Metadata?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<CancellationToken>());
    }
}