using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Shared.Extensions;

public static class FilteringExtensions
{
    public static bool IsValidSortDirection(this string? sortDirection) =>
        sortDirection?.ToLowerInvariant() is "asc" or "ascending" or "0" or "descending" or "1" or "desc" or null;

    public static SortDirection ToSortDirectionEnum(this string? sortDirection) =>
        sortDirection?.ToLowerInvariant() switch
        {
            "asc" or "ascending" or "0" or null => SortDirection.Ascending,
            "desc" or "descending" or "1" => SortDirection.Descending,
            _ => SortDirection.Ascending
        };
}