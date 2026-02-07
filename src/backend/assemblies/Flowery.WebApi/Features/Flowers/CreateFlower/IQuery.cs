namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public interface IQuery
{
    Task CreateFlower(DatabaseModel model, CancellationToken cancellationToken);
}