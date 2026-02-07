using System.ComponentModel;
using Flowery.WebApi.Shared.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed record Request : OrderedPaginationParams
{
    [FromQuery(Name = "sortBy")]
    [DefaultValue(SortField.Name)]
    public SortField? SortBy { get; init; } = SortField.Name;

    public string? Category { get; init; }
}

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator(IValidator<OrderedPaginationParams> paginationValidator)
    {
        Include(paginationValidator);
        
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage("Invalid sort field.");
    }
}