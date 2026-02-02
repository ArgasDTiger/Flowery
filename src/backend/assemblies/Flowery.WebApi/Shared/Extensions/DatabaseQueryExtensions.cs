using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Shared.Extensions;

public static class DatabaseQueryExtensions
{
    private const string Asc = "ASC";
    private const string Desc = "DESC";

    public static string ToSqlOrderDirection(this SortDirection sortDirection) =>
        sortDirection == SortDirection.Asc ? Asc : Desc;

    public static string ToSqlOrderDirection(this SortDirection? sortDirection,
        SortDirection defaultSortDirection = SortDirection.Asc) => sortDirection is null
        ? defaultSortDirection.ToSqlOrderDirection()
        : sortDirection.Value.ToSqlOrderDirection();

    public static int GetSqlOffset(this PaginationParams paginationParams) =>
        (paginationParams.PageNumber - 1) * paginationParams.PageSize;
}