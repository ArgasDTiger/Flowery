namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public interface IQuery
{
    Task<OneOf<Success, Error>> DeleteFlowerById(Guid id, CancellationToken cancellationToken);
    Task<OneOf<Success, Error>> DeleteFlowerBySlug(string slug, CancellationToken cancellationToken);
}