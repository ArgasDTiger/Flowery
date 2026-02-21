using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Categories.DeleteCategory;

public interface IHandler
{
    Task<OneOf<Success, NotFound>> DeleteCategory(string slug, CancellationToken cancellationToken);
}