namespace CountryService.Grpc.Tests.CountryGrpcServiceTests;

public class UpdateTests : CountryGrpcServiceTestsBase
{
    [Fact]
    public async Task ShouldReturnEmpty_WhenCountryWasUpdated()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryUpdateRequest(1);
        CountryService.UpdateAsync(Arg.Is<UpdateCountryModel>(x =>
                x.Id == request.Id
                && x.Description == request.Description))
            .Returns(true);

        // Act
        var result = await GrpcService.Update(request, ServerCallContext);

        // Assert
        result.Should().BeOfType<Empty>();
        await CountryService.Received(1)
            .UpdateAsync(Arg.Is<UpdateCountryModel>(x =>
                x.Id == request.Id
                && x.Description == request.Description));
    }

    [Fact]
    public async Task ShouldThrowNotFound_WhenCountryWasNotUpdated()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryUpdateRequest(123);
        CountryService.UpdateAsync(Arg.Any<UpdateCountryModel>()).Returns(false);

        // Act
        var act = async () => await GrpcService.Update(request, ServerCallContext);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Which.Status.Detail.Should().Contain("Country with Id 123 hasn't been updated.");
    }
}