namespace CountryService.Grpc.Tests.CountryGrpcServiceTests;

public class GetTests : CountryGrpcServiceTestsBase
{
    [Fact]
    public async Task ShouldReturnCountryReply_WhenCountryExists()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(1);
        var model = TestDataFactory.CreateCountryModel(request.Id);

        CountryService.GetAsync(request.Id).Returns(model);

        // Act
        var result = await GrpcService.Get(request, ServerCallContext);

        // Assert
        result.Id.Should().Be(model.Id);
        result.Name.Should().Be(model.Name);
    }
    
    [Fact]
    public async Task ShouldThrowNotFound_WhenCountryDoesNotExist()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(404);
        CountryService.GetAsync(request.Id).Returns((CountryModel?)null);

        // Act
        var act = async () => await GrpcService.Get(request, ServerCallContext);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Which.Status.Detail.Should().Contain("Country with Id 404 hasn't been found");
    }
}