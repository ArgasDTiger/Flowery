using Flowery.Domain.Enums;
using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly TranslationConfiguration _translationConfiguration;

    public Handler(IQuery query, IOptions<TranslationConfiguration> translationSettings)
    {
        _query = query;
        _translationConfiguration = translationSettings.Value;
    }

    public async Task<Guid> CreateFlower(Request request,
        CancellationToken cancellationToken)
    {
        DatabaseModel dbModel = DatabaseModel.FromRequest(request);
        LanguageCode defaultLanguageName = _translationConfiguration.SlugDefaultLanguage;
        string flowerName = dbModel.FlowerNames
            .Where(fn => fn.LanguageCode == defaultLanguageName)
            .AsValueEnumerable()
            .Select(fn => fn.Name)
            .FirstOrDefault() ?? dbModel.FlowerNames[0].Name;

        string slug = flowerName.GenerateSlug();

        // TODO: verify if slug exists
        dbModel.Slug = slug;

        return await _query.CreateFlower(dbModel, cancellationToken);
    }
}