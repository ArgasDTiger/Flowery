using System.Text.Json;
using System.Text.Json.Serialization;
using Flowery.WebApi.Shared.Extensions;

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
        return reader.GetString().ToSortDirectionEnum();
    }

    public override void Write(Utf8JsonWriter writer, SortDirection value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value == SortDirection.Ascending ? "asc" : "desc");
    }
}