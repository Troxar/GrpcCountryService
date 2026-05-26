namespace CountryService.Grpc.Tests.CountryGrpcServiceTests;

public sealed class CreateTests : CountryGrpcServiceTestsBase
{
    [Fact]
    public async Task ShouldCreateCountriesAndWriteReplies_WhenRequestStreamContainsItems()
    {
        // Arrange
        var requests = new[]
        {
            TestDataFactory.CreateCountryCreateRequest(),
            TestDataFactory.CreateCountryCreateRequest()
        };

        for (var i = 0; i < requests.Length; i++)
        {
            var name = requests[i].Name;
            CountryService.CreateAsync(Arg.Is<CreateCountryModel>(x => x.Name == name), Arg.Any<CancellationToken>())
                .Returns(i + 1);
        }

        var requestStream = TestDataFactory.CreateAsyncStreamReader(requests);
        var responseStream = TestDataFactory.CreateServerStreamWriter<CountryCreateReply>();

        // Act
        await GrpcService.Create(requestStream, responseStream, ServerCallContext);

        // Assert
        responseStream.Messages.Should().HaveCount(requests.Length);

        for (var i = 0; i < requests.Length; i++)
        {
            var request = requests[i];
            var message = responseStream.Messages[i];

            message.Id.Should().Be(i + 1);
            message.Name.Should().Be(request.Name);
            await CountryService.Received(1)
                .CreateAsync(Arg.Is<CreateCountryModel>(x =>
                    x.Name == request.Name
                    && x.Description == request.Description), Arg.Any<CancellationToken>());
        }
    }
}