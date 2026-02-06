using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.Shared.Enums;
using Flowery.Shared.Exceptions;
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
        var flowerById = await _query.GetFlowerIdBySlug(flowerId, cancellationToken);
        if (flowerById is null) return StaticResults.NotFound;

        LanguageCode defaultLanguageName = _translationConfiguration.SlugDefaultLanguage;
        string flowerName = request.FlowerNames
                                .AsValueEnumerable()
                                .Where(fn => fn.LanguageCode == defaultLanguageName)
                                .Select(fn => fn.Name)
                                .FirstOrDefault() ?? throw new DefaultLanguageTranslationMissingException(_translationConfiguration.SlugDefaultLanguage);

        bool nameChanged = string.Equals(flowerName, flowerById.Name, StringComparison.OrdinalIgnoreCase);
        DatabaseModel dbModel = new(
            Id: flowerById.Id,
            OldSlug: flowerId,
            NewSlug: nameChanged ? flowerName.GenerateSlug(_translationConfiguration.SlugDefaultLanguage) : null,
            Price: request.Price,
            Description: request.Description,
            FlowerNames: request.FlowerNames,
            NameChanged: nameChanged);

        return await _query.UpdateFlower(dbModel, cancellationToken);
    }
}