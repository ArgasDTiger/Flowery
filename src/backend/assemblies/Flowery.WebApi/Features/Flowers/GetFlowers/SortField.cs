using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

[JsonConverter(typeof(SortFieldConverter))]
public enum SortField : byte
{
    Name = 0,
    Price = 1
}

// TODO: just use enum description attribute?
public sealed class SortFieldConverter : JsonConverter<SortField>
{
    public override SortField Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString()?.ToLowerInvariant();
        return value switch
        {
            "name" or null => SortField.Name,
            "price" => SortField.Price,
            _ => throw new JsonException($"Invalid SortField: {value}.")
        };
    }

    public override void Write(Utf8JsonWriter writer, SortField value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}