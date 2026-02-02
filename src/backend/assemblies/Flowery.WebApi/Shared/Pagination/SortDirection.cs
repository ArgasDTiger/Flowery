using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Flowery.WebApi.Shared.Pagination;

[JsonConverter(typeof(JsonStringEnumConverter<SortDirection>))]
public enum SortDirection : byte
{
    [EnumMember(Value = "asc")]
    Asc = 0,
    [EnumMember(Value = "desc")]
    Desc = 1
}