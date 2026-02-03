using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public interface IQuery
{
    Task<bool> DoesFlowerExist(Guid id, CancellationToken cancellationToken);
    Task<bool> DoesFlowerExist(string slug, CancellationToken cancellationToken);
    Task<OneOf<Success, NotFound>> UpdateFlower(DatabaseModel model, CancellationToken cancellationToken);
}