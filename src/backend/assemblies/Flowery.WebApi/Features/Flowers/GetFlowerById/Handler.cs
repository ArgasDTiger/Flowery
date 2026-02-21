using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.Shared.Enums;

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
        Response? flower = await _query.GetFlowerBySlug(id, LanguageCode.UA, cancellationToken);
        return flower is null ? StaticResults.NotFound : flower;
    }
}