namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public interface IQuery
{
    Task<List<Response>> GetFlowers(Request paginationParams, CancellationToken cancellationToken);
}