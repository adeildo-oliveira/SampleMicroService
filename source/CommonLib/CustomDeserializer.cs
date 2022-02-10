using Confluent.Kafka;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CommonLib;

public struct CustomDeserializer<T> : IDeserializer<T>
{
    private static readonly JsonSerializerOptions options = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => !isNull ? JsonSerializer.Deserialize<T>(data, options) : default;
}
