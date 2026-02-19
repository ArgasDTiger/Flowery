namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public interface IHandler
{
    Task<string> CreateFlower(HandlerModel request, CancellationToken cancellationToken);
}