using Flowery.WebApi.Shared.ActionResults.Static;

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
        bool isIdGuid = Guid.TryParse(id, out Guid guidId);

        Response? flower = isIdGuid
            ? await _query.GetFlowerById(guidId, cancellationToken)
            : await _query.GetFlowerBySlug(id, cancellationToken);

        return flower is null ? StaticResults.NotFound : flower;
    }
}