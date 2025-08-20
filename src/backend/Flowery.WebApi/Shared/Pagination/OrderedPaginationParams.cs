using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Flowery.WebApi.Shared.Pagination;

public abstract class OrderedPaginationParams : PaginationParams
{
    [FromQuery(Name = "sortDirection")]
    [DefaultValue(nameof(SortDirection.Ascending))]
    public string? SortDirectionString { private get; init; } = nameof(SortDirection.Ascending);

    [BindNever]
    public SortDirection SortDirection => SortDirectionString?.ToLowerInvariant() switch
    {
        "asc" or "ascending" or "0" => SortDirection.Ascending,
        "desc" or "descending" or "1" => SortDirection.Descending,
        _ => throw new Exception($"Cannot sort in direction {SortDirectionString}.")
    };
}