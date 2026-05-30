using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

[JsonConverter(typeof(JsonStringEnumConverter<SortField>))]
public enum SortField : byte
{
    [EnumMember(Value = "name")] Name = 1,
    [EnumMember(Value = "price")] Price = 2
}

public readonly struct SortFieldQuery : IParsable<SortFieldQuery>
{
    public SortField Value { get; }

    public SortFieldQuery(SortField value)
    {
        Value = value;
    }

    public static implicit operator SortField(SortFieldQuery query) => query.Value;
    public static implicit operator SortFieldQuery(SortField value) => new(value);

    public static SortFieldQuery Parse(string value, IFormatProvider? provider)
    {
        return TryParse(value, provider, out var result)
            ? result
            : throw new FormatException($"Invalid {nameof(SortField)}: {value}");
    }

    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, out SortFieldQuery result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        var valueSpan = value.AsSpan();
        if (valueSpan.Equals(nameof(SortField.Name), StringComparison.OrdinalIgnoreCase))
        {
            result = new SortFieldQuery(SortField.Name);
            return true;
        }

        if (valueSpan.Equals(nameof(SortField.Price), StringComparison.OrdinalIgnoreCase))
        {
            result = new SortFieldQuery(SortField.Price);
            return true;
        }

        result = default;
        return false;
    }
}