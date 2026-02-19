using Flowery.Shared.Enums;

namespace Flowery.WebApi.Features.Categories.GetCategories;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<ImmutableArray<Response>> GetCategories(LanguageCode languageCode, CancellationToken cancellationToken)
    {
        return await _query.GetCategories(LanguageCode.UA, cancellationToken);
    }
}