using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Shared.Extensions;

public static class FilteringExtensions
{
    extension(string? sortDirection)
    {
        public bool IsValidSortDirection() =>
            sortDirection?.ToLowerInvariant() is "asc" or "desc" or null;

        public SortDirection ToSortDirectionEnum() =>
            sortDirection?.ToLowerInvariant() switch
            {
                "asc" or null => SortDirection.Ascending,
                "desc" => SortDirection.Descending,
                _ => SortDirection.Ascending
            };
    }
}