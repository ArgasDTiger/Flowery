using System.ComponentModel;
using Flowery.WebApi.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Flowery.WebApi.Shared.Pagination;

public abstract record OrderedPaginationParams : PaginationParams
{
    [FromQuery(Name = "sortDirection")]
    [DefaultValue(nameof(SortDirection.Ascending))]
    public string? SortDirectionString { private get; init; } = nameof(SortDirection.Ascending);

    [BindNever]
    public SortDirection SortDirection => SortDirectionString.ToSortDirectionEnum();
}