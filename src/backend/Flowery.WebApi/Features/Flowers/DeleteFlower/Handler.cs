using Flowery.WebApi.Shared.ActionResults.Static;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<OneOf<Success, NotFound>> DeleteFlower(string id, CancellationToken cancellationToken)
    {
        bool isIdGuid = Guid.TryParse(id, out Guid guidId);

        var result = isIdGuid
            ? await _query.DeleteFlowerById(guidId, cancellationToken)
            : await _query.DeleteFlowerBySlug(id, cancellationToken);

        return result.Match<OneOf<Success, NotFound>>(
            _ => StaticResults.Success,
            _ => StaticResults.NotFound);
    }
}