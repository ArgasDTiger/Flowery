using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public interface IHandler
{
    Task<OneOf<Response, NotFound>> GetFlower<T>(T id, CancellationToken cancellationToken);
}