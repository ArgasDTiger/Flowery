using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Categories.GetCategoryById;

public interface IHandler
{
    Task<OneOf<Response, NotFound>> GetCategoryBySlug(string slug, CancellationToken cancellationToken);
}