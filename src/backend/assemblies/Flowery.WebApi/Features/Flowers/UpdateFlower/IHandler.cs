using Flowery.Domain.ActionResults;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public interface IHandler
{
    Task<OneOf<Success, NotFound>> UpdateFlower(string flowerId, Request request, CancellationToken cancellationToken);
}