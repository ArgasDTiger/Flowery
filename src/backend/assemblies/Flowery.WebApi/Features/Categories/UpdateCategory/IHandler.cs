using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Categories.UpdateCategory;

public interface IHandler
{
    Task<OneOf<Success, NotFound>> UpdateCategory(string slug, Request request, CancellationToken cancellationToken);
}