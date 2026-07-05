namespace CountryShared.Grpc.Options;

public sealed class GrpcOptions
{
    public const string SectionName = "Grpc";
    public int MaxReceiveMessageSizeMb { get; init; } = 6;
    public int MaxSendMessageSizeMb { get; init; } = 6;
    public int DefaultCallTimeoutSeconds { get; init; } = 5;
    public int MaxReceiveMessageSize => MaxReceiveMessageSizeMb * 1024 * 1024;
    public int MaxSendMessageSize => MaxSendMessageSizeMb * 1024 * 1024;
    public bool UseBrotliCompression { get; init; }
    public TimeSpan DefaultCallTimeout => TimeSpan.FromSeconds(DefaultCallTimeoutSeconds);
}