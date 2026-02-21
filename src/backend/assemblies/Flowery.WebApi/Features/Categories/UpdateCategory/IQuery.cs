using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Categories.UpdateCategory;

public interface IQuery
{
    Task<CategoryBySlugModel?> GetCategoryBySlug(string slug, CancellationToken cancellationToken);
    Task<OneOf<Success, NotFound>> UpdateCategory(DatabaseModel model, CancellationToken cancellationToken);
}