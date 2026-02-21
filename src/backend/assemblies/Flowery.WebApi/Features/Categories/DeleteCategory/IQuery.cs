using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Categories.DeleteCategory;

public interface IQuery
{
    Task<Guid?> GetCategoryIdBySlug(string slug, CancellationToken cancellationToken);
    Task<OneOf<Success, NotFound>> DeleteCategoryById(Guid categoryId, CancellationToken cancellationToken);
}