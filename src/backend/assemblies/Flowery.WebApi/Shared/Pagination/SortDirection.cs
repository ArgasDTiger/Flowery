using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Flowery.WebApi.Shared.Pagination;

[JsonConverter(typeof(JsonStringEnumConverter<SortDirection>))]
public enum SortDirection : byte
{
    [EnumMember(Value = "asc")] Asc = 0,
    [EnumMember(Value = "desc")] Desc = 1
}

public readonly struct SortDirectionQuery : IParsable<SortDirectionQuery>
{
    public SortDirection Value { get; }

    public SortDirectionQuery(SortDirection value)
    {
        Value = value;
    }

    public static implicit operator SortDirection(SortDirectionQuery query) => query.Value;
    public static implicit operator SortDirectionQuery(SortDirection value) => new(value);

    public static SortDirectionQuery Parse(string value, IFormatProvider? provider)
    {
        return TryParse(value, provider, out var result)
            ? result
            : throw new FormatException($"Invalid {nameof(SortDirection)}: {value}");
    }

    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider,
        out SortDirectionQuery result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        var valueSpan = value.AsSpan();
        if (valueSpan.Equals(nameof(SortDirection.Asc), StringComparison.OrdinalIgnoreCase))
        {
            result = new SortDirectionQuery(SortDirection.Asc);
            return true;
        }

        if (valueSpan.Equals(nameof(SortDirection.Desc), StringComparison.OrdinalIgnoreCase))
        {
            result = new SortDirectionQuery(SortDirection.Desc);
            return true;
        }

        result = default;
        return false;
    }
}