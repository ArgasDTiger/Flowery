using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<PaginatedResponse<Response>> GetFlowers(Request request,
        CancellationToken cancellationToken)
    {
        return await _query.GetFlowers(request, cancellationToken);
    }
}