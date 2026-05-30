using FluentValidation;

namespace Flowery.WebApi.Shared.Pagination;

public abstract record OrderedPaginationParams : PaginationParams
{
    public SortDirectionQuery? SortDirection { get; init; } = Pagination.SortDirection.Asc;
}

public sealed class OrderedPaginationParamsValidator : AbstractValidator<OrderedPaginationParams>
{
    public OrderedPaginationParamsValidator(IValidator<PaginationParams> paginationValidator)
    {
        Include(paginationValidator);
    }
}