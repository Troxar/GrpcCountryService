namespace CountryShared.Extensions;

public static class GrpcChannelOptionsExtensions
{
    extension(GrpcChannelOptions options)
    {
        public GrpcChannelOptions ConfigureGrpcChannel(GrpcOptions? grpcOptions = null,
            bool useBrotliCompression = false)
        {
            grpcOptions ??= new GrpcOptions();

            options.MaxReceiveMessageSize = grpcOptions.MaxReceiveMessageSize;
            options.MaxSendMessageSize = grpcOptions.MaxSendMessageSize;

            if (useBrotliCompression)
                options.CompressionProviders = new List<ICompressionProvider>
                {
                    new BrotliCompressionProvider()
                };

            return options;
        }
    }
}