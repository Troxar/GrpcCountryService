namespace CountryService.Grpc.IntegrationTests.ValidationTests;

public sealed class DeleteTests(CountryGrpcIntegrationFixture fixture) : ValidationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnInvalidArgument_WhenIdIsZero()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(0);

        // Act
        var act = async () => await Fixture.Client.DeleteAsync(request, cancellationToken: CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.InvalidArgument);
        exception.Which.Status.Detail.Should().Contain("Id");
    }
}