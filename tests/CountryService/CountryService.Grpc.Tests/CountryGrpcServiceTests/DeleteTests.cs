namespace CountryService.Grpc.Tests.CountryGrpcServiceTests;

public class DeleteTests : CountryGrpcServiceTestsBase
{
    [Fact]
    public async Task ShouldReturnEmpty_WhenCountryWasDeleted()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(1);
        CountryService.DeleteAsync(request.Id).Returns(true);

        // Act
        var result = await GrpcService.Delete(request, ServerCallContext);

        // Assert
        result.Should().BeOfType<Empty>();
        await CountryService.Received(1).DeleteAsync(request.Id);
    }

    [Fact]
    public async Task ShouldThrowNotFound_WhenCountryWasNotDeleted()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(123);
        CountryService.DeleteAsync(request.Id).Returns(false);

        // Act
        var act = async () => await GrpcService.Delete(request, ServerCallContext);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Which.Status.Detail.Should().Contain("Country with Id 123 hasn't been deleted");
    }
}