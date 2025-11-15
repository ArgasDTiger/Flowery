using System.ComponentModel;
using Flowery.WebApi.Shared.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed record Request : OrderedPaginationParams
{
    [FromQuery(Name = "sortBy")]
    [DefaultValue(nameof(SortField.Name))]
    public string? SortFieldString { get; init; } = nameof(SortField.Name);

    [BindNever]
    public SortField SortField => SortFieldString?.ToLowerInvariant() switch
    {
        "name" or null => SortField.Name,
        "price" => SortField.Price,
        _ => SortField.Name
    };
}

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator(IValidator<OrderedPaginationParams> paginationValidator)
    {
        Include(paginationValidator);
        
        RuleFor(x => x.SortFieldString)
            .Must(x => x?.ToLowerInvariant() is "name" or "price" or null)
            .WithMessage("Invalid sort field.");
    }
}