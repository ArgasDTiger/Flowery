using System.ComponentModel;
using Flowery.WebApi.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Flowery.WebApi.Shared.Pagination;

public abstract record OrderedPaginationParams : PaginationParams
{
    [FromQuery(Name = "sortDirection")]
    [DefaultValue(nameof(SortDirection.Ascending))]
    public string? SortDirectionString { get; init; } = nameof(SortDirection.Ascending); // TODO: change to "asc"

    [BindNever]
    public SortDirection SortDirection => SortDirectionString.ToSortDirectionEnum();
}

public sealed class OrderedPaginationParamsValidator : AbstractValidator<OrderedPaginationParams>
{
    public OrderedPaginationParamsValidator(IValidator<PaginationParams> paginationValidator)
    {
        Include(paginationValidator);
        
        RuleFor(x => x.SortDirectionString)
            .Must(x => x.IsValidSortDirection())
            .WithMessage("Sort direction is invalid.");
    }
}