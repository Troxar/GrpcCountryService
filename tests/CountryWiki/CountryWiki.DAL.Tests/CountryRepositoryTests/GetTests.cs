namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public class GetTests : CountryRepositoryTestsBase
{
    [Fact]
    public async Task ShouldReturnMappedCountry_WhenCountryExists()
    {
        // Arrange
        var reply = TestDataFactory.CreateCountryReply(1);
        reply.Languages.AddRange(["English", "French"]);

        Client.GetAsync(Arg.Any<CountryIdRequest>(),
                Arg.Any<Metadata?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateUnaryCall(reply));

        // Act
        var result = await Repository.GetAsync(reply.Id);

        // Assert
        _ = Client.Received(1).GetAsync(
            Arg.Is<CountryIdRequest>(x => x.Id == reply.Id),
            Arg.Any<Metadata?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<CancellationToken>());

        result.Should().NotBeNull();
        result.Id.Should().Be(reply.Id);
        result.Name.Should().Be(reply.Name);
        result.Description.Should().Be(reply.Description);
        result.FlagUri.Should().Be(reply.FlagUri);
        result.Anthem.Should().Be(reply.Anthem);
        result.CapitalCity.Should().Be(reply.CapitalCity);
        result.Languages.Should().BeEquivalentTo(reply.Languages);
    }

    [Fact]
    public async Task ShouldPropagateRpcException_WhenGrpcClientThrowsNotFound()
    {
        // Arrange
        var status = new Status(StatusCode.NotFound, "Country has not been found");
        var rpcException = new RpcException(status);

        Client.GetAsync(
                Arg.Any<CountryIdRequest>(),
                Arg.Any<Metadata?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<CancellationToken>())
            .Returns(TestDataFactory.CreateFailedUnaryCall<CountryReply>(rpcException));

        // Act
        var act = async () => await Repository.GetAsync(999);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(status.StatusCode);
    }
}