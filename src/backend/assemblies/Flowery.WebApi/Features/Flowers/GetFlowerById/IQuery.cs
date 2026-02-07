namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public interface IQuery
{
    Task<Response?> GetFlowerBySlug(string slug, CancellationToken cancellationToken);
}