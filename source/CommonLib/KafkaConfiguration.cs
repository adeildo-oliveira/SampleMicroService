using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CommonLib;

public interface IKafkaConfig
{
    ConsumerConfig ConsumerConfiguration();
    ProducerConfig ProducerConfiguration();
    string Topic { get; }
    string DeadLetter { get; }
}

public class KafkaConfig : IKafkaConfig
{
    private readonly KafkaOption _kafkaOption;

    public KafkaConfig(IOptions<KafkaOption> options) => _kafkaOption = options.Value;

    public string Topic => _kafkaOption.Topic;
    public string DeadLetter => _kafkaOption.DeadLetter;

    public ConsumerConfig ConsumerConfiguration() => new()
    {
        BootstrapServers = _kafkaOption.BootstrapServers,
        GroupId = _kafkaOption.GroupId,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnablePartitionEof = _kafkaOption.EnablePartitionEof,
        EnableAutoCommit = _kafkaOption.EnableAutoCommit,
        Acks = Acks.Leader
    };

    public ProducerConfig ProducerConfiguration() => new()
    {
        BootstrapServers = _kafkaOption.BootstrapServers,
        Acks = Acks.Leader,
        MessageSendMaxRetries = _kafkaOption.MessageSendMaxRetries,
        MessageTimeoutMs = _kafkaOption.MessageTimeoutMs,
        RequestTimeoutMs = _kafkaOption.RequestTimeoutMs,
        SocketTimeoutMs = _kafkaOption.SocketTimeoutMs
    };
}