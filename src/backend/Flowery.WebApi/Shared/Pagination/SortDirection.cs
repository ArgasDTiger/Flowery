using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flowery.WebApi.Shared.Pagination;

[JsonConverter(typeof(SortDirectionConverter))]
public enum SortDirection : byte
{
    Ascending = 0,
    Descending = 1
}

public sealed class SortDirectionConverter : JsonConverter<SortDirection>
{
    public override SortDirection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString()?.ToLowerInvariant();
        return value switch
        {
            "asc" => SortDirection.Ascending,
            "ascending" => SortDirection.Ascending,
            "0" => SortDirection.Ascending,
            "desc" => SortDirection.Descending,
            "descending" => SortDirection.Descending,
            "1" => SortDirection.Descending,
            _ => throw new JsonException($"Invalid SortDirection: {value}.")
        };
    }

    public override void Write(Utf8JsonWriter writer, SortDirection value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value == SortDirection.Ascending ? "asc" : "desc");
    }
}