namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public interface IHandler
{
    Task<Guid> CreateFlower(Request request, CancellationToken cancellationToken);
}