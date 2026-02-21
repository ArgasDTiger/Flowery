using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;

namespace Flowery.WebApi.Features.Categories.DeleteCategory;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<OneOf<Success, NotFound>> DeleteCategory(string slug, CancellationToken cancellationToken)
    {
        Guid? categoryId = await _query.GetCategoryIdBySlug(slug, cancellationToken);
        if (categoryId is null) return StaticResults.NotFound;

        return await _query.DeleteCategoryById(categoryId.Value, cancellationToken);
    }
}