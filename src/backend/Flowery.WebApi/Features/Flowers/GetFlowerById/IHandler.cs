namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public interface IHandler
{
    Task<OneOf<Response, NotFound>> GetFlowerById(string id, CancellationToken cancellationToken);
}