namespace CommonLib;

public class KafkaOption
{
    public string? BootstrapServers { get; init; }
    public string? GroupId { get; init; }
    public bool EnablePartitionEof { get; init; }
    public bool EnableAutoCommit { get; init; }
    public string? Topic { get; init; }
    public string? DeadLetter { get; init; }
    public int MessageSendMaxRetries { get; init; }
    public int MessageTimeoutMs { get; init; }
    public int RequestTimeoutMs { get; init; }
    public int SocketTimeoutMs { get; init; }
}
