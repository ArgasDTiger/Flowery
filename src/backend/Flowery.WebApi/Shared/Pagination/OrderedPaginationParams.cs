using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Shared.Pagination;

public abstract class OrderedPaginationParams : PaginationParams
{
    [FromQuery(Name = "orderDirection")]
    public SortDirection SortDirection { get; init; }
}