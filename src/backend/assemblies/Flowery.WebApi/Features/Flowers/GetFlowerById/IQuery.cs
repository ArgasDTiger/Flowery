namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public interface IQuery
{
    Task<Response?> GetFlowerById(Guid id, CancellationToken cancellationToken);
    Task<Response?> GetFlowerBySlug(string slug, CancellationToken cancellationToken);
}