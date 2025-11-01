namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public interface IQuery
{
    Task<int> CreateFlower(DatabaseModel model, CancellationToken cancellationToken);
}