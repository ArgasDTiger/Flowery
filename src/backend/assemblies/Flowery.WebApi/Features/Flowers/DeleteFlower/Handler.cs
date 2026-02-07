using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<OneOf<Success, NotFound>> DeleteFlower(string slug, CancellationToken cancellationToken)
    {
        return await _query.DeleteFlowerBySlug(slug, cancellationToken);
    }
}