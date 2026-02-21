using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;

namespace Flowery.WebApi.Features.Categories.GetCategoryById;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<OneOf<Response, NotFound>> GetCategoryBySlug(string slug, CancellationToken cancellationToken)
    {
        var category = await _query.GetCategoryBySlug(slug, cancellationToken);
        if (category is null) return StaticResults.NotFound;
        return category;
    }
}