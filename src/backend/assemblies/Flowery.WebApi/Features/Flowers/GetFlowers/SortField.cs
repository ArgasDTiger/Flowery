using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

[JsonConverter(typeof(JsonStringEnumConverter<SortField>))]
public enum SortField : byte
{
    [EnumMember(Value = "name")]
    Name = 1,
    [EnumMember(Value = "price")]
    Price = 2
}