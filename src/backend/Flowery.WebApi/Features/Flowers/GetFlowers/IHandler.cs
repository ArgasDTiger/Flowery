using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public interface IHandler
{
    Task<PaginatedResponse<Response>> GetFlowers(Request request, CancellationToken cancellationToken);
}