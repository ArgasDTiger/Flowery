using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Flowery.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<LanguageCode>))]
public enum LanguageCode : byte
{
    [EnumMember(Value = "ua")] UA = 0,
    [EnumMember(Value = "ro")] RO = 1
}