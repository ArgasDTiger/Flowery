using Flowery.Domain.ActionResults;
using Flowery.Domain.ActionResults.Static;
using Flowery.Domain.Enums;
using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Models;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly TranslationConfiguration _translationConfiguration;

    public Handler(IQuery query, IOptions<TranslationConfiguration> translationSettings)
    {
        _query = query;
        _translationConfiguration = translationSettings.Value;
    }

    public async Task<OneOf<Success, NotFound>> UpdateFlower(string flowerId, Request request,
        CancellationToken cancellationToken)
    {
        SlugOrId slugOrId = flowerId.GetSlugOrId();
        // TODO: dont need slug with id, just id, because slug will be generated from name anyways
        SlugWithId? slugWithId = await GetFlowerSlugWithIdIfExists(slugOrId, cancellationToken);

        if (slugWithId is null) return StaticResults.NotFound;

        LanguageCode defaultLanguageName = _translationConfiguration.SlugDefaultLanguage;
        string flowerName = request.FlowerNames
            .Where(fn => fn.LanguageCode == defaultLanguageName)
            .AsValueEnumerable()
            .Select(fn => fn.Name)
            .FirstOrDefault() ?? request.FlowerNames[0].Name;

        // TODO: verify if slug exists
        string slug = flowerName.GenerateSlug();

        return await _query.UpdateFlower(new DatabaseModel(request, slug, slugWithId.Value.Id),
            cancellationToken);
    }

    private async Task<SlugWithId?> GetFlowerSlugWithIdIfExists(SlugOrId slugOrId, CancellationToken cancellationToken)
    {
        if (slugOrId.Id is not null)
        {
            return await _query.GetFlowerById(slugOrId.Id.Value, cancellationToken);
        }

        return await _query.GetFlowerBySlug(slugOrId.Slug!, cancellationToken);
    }
}