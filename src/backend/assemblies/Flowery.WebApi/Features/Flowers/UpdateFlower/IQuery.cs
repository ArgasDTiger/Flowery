using Flowery.Domain.ActionResults;
using Flowery.WebApi.Shared.Models;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public interface IQuery
{
    Task<SlugWithId?> GetFlowerById(Guid id, CancellationToken cancellationToken);
    Task<SlugWithId?> GetFlowerBySlug(string slug, CancellationToken cancellationToken);
    Task<OneOf<Success, NotFound>> UpdateFlower(DatabaseModel model, CancellationToken cancellationToken);
}