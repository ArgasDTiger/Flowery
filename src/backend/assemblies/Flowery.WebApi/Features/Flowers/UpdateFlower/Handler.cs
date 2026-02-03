using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Extensions;
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
        var flowerExistResult = await DoesFlowerExist(flowerId, cancellationToken);
        if (!flowerExistResult.flowerExists) return StaticResults.NotFound;

        LanguageCode defaultLanguageName = _translationConfiguration.SlugDefaultLanguage;
        string flowerName = request.FlowerNames
            .Where(fn => fn.LanguageCode == defaultLanguageName)
            .AsValueEnumerable()
            .Select(fn => fn.Name)
            .FirstOrDefault() ?? request.FlowerNames[0].Name;

        // TODO: verify if slug exists
        string slug = flowerName.GenerateSlug();

        var dbModel = flowerExistResult.flowerId.IsT0
            ? DatabaseModel.CreateWithSlugId(request, slug, flowerExistResult.flowerId.AsT0)
            : DatabaseModel.CreateWithGuidId(request, slug, flowerExistResult.flowerId.AsT1);
        
        return await _query.UpdateFlower(dbModel, cancellationToken);
        
    }

    private async Task<(bool flowerExists, OneOf<string, Guid> flowerId)> DoesFlowerExist(string flowerId,
        CancellationToken cancellationToken)
    {
        bool isIdGuid = Guid.TryParse(flowerId, out Guid guidId);
        if (isIdGuid)
        {
            bool flowerExist = await _query.DoesFlowerExist(guidId, cancellationToken);
            return (flowerExist, guidId);
        }

        bool flowerExists = await _query.DoesFlowerExist(flowerId, cancellationToken);
        return (flowerExists, flowerId);
    }
}