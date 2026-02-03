using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.Shared.Exceptions;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<OneOf<Response, NotFound>> GetFlower<T>(T identifier, CancellationToken cancellationToken)
    {
        Response? flower = identifier switch
        {
            Guid id => await _query.GetFlowerById(id, cancellationToken),
            string slug => await _query.GetFlowerBySlug(slug, cancellationToken),
            _ => throw new InvalidIdentifierException<T>(nameof(String), nameof(Guid))
        };

        return flower is null ? StaticResults.NotFound : flower;
    }
}