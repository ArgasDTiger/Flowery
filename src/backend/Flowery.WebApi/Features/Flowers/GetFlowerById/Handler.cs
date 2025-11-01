using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Models;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }
    
    public async Task<OneOf<Response, NotFound>> GetFlowerById(string id, CancellationToken cancellationToken)
    {
        SlugOrId slugOrId = id.GetSlugOrId();
        Response? flower = await _query.GetFlowerById(slugOrId, cancellationToken);
        return flower is null ? new NotFound() : flower;
    }
}