using Confluent.Kafka;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CommonLib;

public struct CustomSerializer<T> : ISerializer<T>
{
    private static readonly JsonSerializerOptions options = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    public byte[] Serialize(T data, SerializationContext context) => JsonSerializer.SerializeToUtf8Bytes(data, options);
}