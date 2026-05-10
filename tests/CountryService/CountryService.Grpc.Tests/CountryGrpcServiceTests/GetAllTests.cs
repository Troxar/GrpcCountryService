namespace CountryService.Grpc.Tests.CountryGrpcServiceTests;

public class GetAllTests : CountryGrpcServiceTestsBase
{
    [Fact]
    public async Task ShouldWriteAllReplies_WhenCountriesExist()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1),
            TestDataFactory.CreateCountryModel(2)
        };
        CountryService.GetAllAsync().Returns(countries);

        var responseStream = TestDataFactory.CreateServerStreamWriter<CountryReply>();

        // Act
        await GrpcService.GetAll(new Empty(), responseStream, ServerCallContext);

        // Assert
        responseStream.Messages.Should().HaveCount(countries.Length);
        foreach (var country in countries)
            responseStream.Messages.Should().ContainSingle(x =>
                x.Id == country.Id
                && x.Name == country.Name
                && x.Description == country.Description);
    }
}