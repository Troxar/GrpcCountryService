namespace CountryWiki.DAL.Tests.CountryRepositoryTests.CancellationTests;

public sealed class GetAllTests : CancellationTestsBase
{
    [Fact]
    public async Task ShouldStopEnumeration_WhenCancellationRequested()
    {
        // Arrange
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();
        var cancellationToken = cancellationTokenSource.Token;
        var responseStream = new CancellationAwareAsyncStreamReader<CountryReply>();
        Client.GetAll(Arg.Any<Empty>(), Arg.Any<Metadata?>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(CreateServerStreamingCall(responseStream));

        // Act
        var act = async () =>
        {
            await foreach (var _ in Repository.GetAllAsync(cancellationToken))
            {
            }
        };

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();

        responseStream.CapturedCancellationToken.Should().Be(cancellationToken);
        Client.Received(1).GetAll(Arg.Any<Empty>(), Arg.Any<Metadata?>(), Arg.Any<DateTime?>(), cancellationToken);
    }

    private static AsyncServerStreamingCall<TResponse> CreateServerStreamingCall<TResponse>(
        IAsyncStreamReader<TResponse> responseStream)
    {
        return new AsyncServerStreamingCall<TResponse>(responseStream,
            Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess,
            () => [],
            () => { });
    }
}