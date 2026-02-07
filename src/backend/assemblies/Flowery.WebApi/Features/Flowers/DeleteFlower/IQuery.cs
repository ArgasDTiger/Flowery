using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public interface IQuery
{
    Task<OneOf<Success, NotFound>> DeleteFlowerBySlug(string slug, CancellationToken cancellationToken);
}