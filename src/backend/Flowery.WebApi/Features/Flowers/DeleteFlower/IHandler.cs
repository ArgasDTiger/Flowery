namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public interface IHandler
{
    Task<OneOf<Success, NotFound>> DeleteFlower(Guid id, CancellationToken cancellationToken);
}