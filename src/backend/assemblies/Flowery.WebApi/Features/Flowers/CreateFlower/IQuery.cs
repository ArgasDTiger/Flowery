namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public interface IQuery
{
    Task<Guid> CreateFlower(DatabaseModel model, CancellationToken cancellationToken);
}