using System.ComponentModel;
using Flowery.WebApi.Shared.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Request : OrderedPaginationParams
{
    [FromQuery(Name = "sortBy")]
    [DefaultValue(nameof(SortField.Name))]
    public string? SortFieldString { private get; init; } = nameof(SortField.Name);

    [BindNever]
    public SortField SortField => SortFieldString?.ToLowerInvariant() switch
    {
        "price" => SortField.Price,
        "name" => SortField.Name,
        _ => throw new Exception($"Cannot sort by {SortFieldString}.")
    };
}

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator(IValidator<PaginationParams> paginationValidator)
    {
        Include(paginationValidator);
    }
}