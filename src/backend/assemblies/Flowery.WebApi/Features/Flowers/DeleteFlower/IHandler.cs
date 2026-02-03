using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public interface IHandler
{
    Task<OneOf<Success, NotFound>> DeleteFlower<T>(T id, CancellationToken cancellationToken);
}