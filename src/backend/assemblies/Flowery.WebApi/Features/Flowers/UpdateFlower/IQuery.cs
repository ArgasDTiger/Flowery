using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public interface IQuery
{
    Task<FlowerBySlugModel?> GetFlowerIdBySlug(string slug, CancellationToken cancellationToken);
    Task<OneOf<Success, NotFound>> UpdateFlower(DatabaseModel model, CancellationToken cancellationToken);
}