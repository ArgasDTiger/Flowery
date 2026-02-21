using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Categories.CreateCategory;

public interface IHandler
{
    Task<OneOf<string, Error>> CreateCategory(Request request, CancellationToken cancellationToken);
}