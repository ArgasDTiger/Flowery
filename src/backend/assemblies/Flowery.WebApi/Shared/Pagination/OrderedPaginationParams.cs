using System.ComponentModel;
using FluentValidation;

namespace Flowery.WebApi.Shared.Pagination;

public abstract record OrderedPaginationParams : PaginationParams
{
    [DefaultValue(Pagination.SortDirection.Asc)]
    public SortDirection? SortDirection { get; init; } = Pagination.SortDirection.Asc;
}

public sealed class OrderedPaginationParamsValidator : AbstractValidator<OrderedPaginationParams>
{
    public OrderedPaginationParamsValidator(IValidator<PaginationParams> paginationValidator)
    {
        Include(paginationValidator);
        
        RuleFor(x => x.SortDirection)
            .IsInEnum()
            .WithMessage("Sort direction is invalid.");
    }
}