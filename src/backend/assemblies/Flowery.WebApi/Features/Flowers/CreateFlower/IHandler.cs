namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public interface IHandler
{
    Task<string> CreateFlower(Request request, CancellationToken cancellationToken);
}