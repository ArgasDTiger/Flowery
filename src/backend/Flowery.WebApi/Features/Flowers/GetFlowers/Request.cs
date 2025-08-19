using Flowery.WebApi.Shared.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Request : OrderedPaginationParams
{
    [FromQuery(Name = "orderBy")]
    public SortField SortField { get; init; } = SortField.Name;
}

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        Include(new PaginationParamsValidator());
    }
}