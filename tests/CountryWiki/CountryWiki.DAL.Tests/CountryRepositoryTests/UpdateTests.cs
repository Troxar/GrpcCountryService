namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public sealed class UpdateTests : CountryRepositoryTestsBase
{
    [Fact]
    public async Task ShouldSendUpdateRequest()
    {
        // Arrange
        Client.UpdateAsync(Arg.Any<CountryUpdateRequest>(),
                Arg.Any<Metadata?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateUnaryCall(new Empty()));
        var model = TestDataFactory.CreateUpdateCountryModel(1);

        // Act
        await Repository.UpdateAsync(model);

        // Assert
        _ = Client.Received(1).UpdateAsync(
            Arg.Is<CountryUpdateRequest>(x =>
                x.Id == model.Id
                && x.Description == model.Description),
            Arg.Any<Metadata?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<CancellationToken>());
    }
}