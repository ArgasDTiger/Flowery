using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Shared.Extensions;

public static class FilteringExtensions
{
    public static SortDirection ToSortDirectionEnum(this string? sortDirection) => sortDirection?.ToLowerInvariant() switch
    {
        "asc" or "ascending" or "0" or null => SortDirection.Ascending,
        "desc" or "descending" or "1" => SortDirection.Descending,
        _ => throw new ArgumentOutOfRangeException($"Invalid SortDirection: {sortDirection}.")
    };
}