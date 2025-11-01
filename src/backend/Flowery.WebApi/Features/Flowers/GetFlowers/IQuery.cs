namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public interface IQuery
{
    Task<ImmutableArray<Response>> GetFlowers(Request paginationParams, CancellationToken cancellationToken);
}