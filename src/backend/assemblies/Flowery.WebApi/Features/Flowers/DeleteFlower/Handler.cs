using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.Shared.Exceptions;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<OneOf<Success, NotFound>> DeleteFlower<T>(T identifier, CancellationToken cancellationToken)
    {
        var result = identifier switch
        {
            Guid id => await _query.DeleteFlowerById(id, cancellationToken),
            string slug => await _query.DeleteFlowerBySlug(slug, cancellationToken),
            _ => throw new InvalidIdentifierException<T>(nameof(String), nameof(Guid))
        };

        return result.Match<OneOf<Success, NotFound>>(
            _ => StaticResults.Success,
            _ => StaticResults.NotFound);
    }
}