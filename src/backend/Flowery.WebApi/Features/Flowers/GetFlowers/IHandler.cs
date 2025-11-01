namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public interface IHandler
{
    Task<ImmutableArray<Response>> GetFlowers(Request request, CancellationToken cancellationToken);
}