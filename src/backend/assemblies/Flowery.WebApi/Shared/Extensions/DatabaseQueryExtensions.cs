using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Shared.Extensions;

public static class DatabaseQueryExtensions
{
    public static string ToSqlOrderDirection(this SortDirection sortDirection) =>
        sortDirection == SortDirection.Ascending ? "ASC" : "DESC";

    public static int GetSqlOffset(this PaginationParams paginationParams) =>
        (paginationParams.PageNumber - 1) * paginationParams.PageSize;
}