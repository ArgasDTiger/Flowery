using System.ComponentModel;
using Flowery.WebApi.Shared.Pagination;
using FluentValidation;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed record Request : OrderedPaginationParams
{
    public SortFieldQuery? SortBy { get; init; } = SortField.Name;

    public string? Category { get; init; }
}

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator(IValidator<OrderedPaginationParams> paginationValidator)
    {
        Include(paginationValidator);
    }
}