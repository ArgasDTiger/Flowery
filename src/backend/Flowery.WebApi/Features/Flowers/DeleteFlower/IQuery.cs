namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public interface IQuery
{
    Task<int> DeleteFlower(Guid id, CancellationToken cancellationToken);
}