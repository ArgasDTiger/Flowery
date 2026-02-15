using Flowery.Shared.Enums;
using Flowery.Shared.Exceptions;
using Flowery.WebApi.Features.Flowers.Helpers;
using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly IFlowerImageProcessor _imageProcessor;
    private readonly TranslationConfiguration _translationConfiguration;

    public Handler(IQuery query, IOptions<TranslationConfiguration> translationSettings, IFlowerImageProcessor imageProcessor)
    {
        _query = query;
        _imageProcessor = imageProcessor;
        _translationConfiguration = translationSettings.Value;
    }

    public async Task<string> CreateFlower(Request request,
        CancellationToken cancellationToken)
    {
        // TODO: Handler should NOT acceess IFormFile, move to endpoint
        Stream stream = request.PrimaryImage.OpenReadStream();
        await _imageProcessor.ProcessImage(stream, request.PrimaryImage.FileName, request.PrimaryImage.ContentType,
            cancellationToken);
        // DatabaseModel dbModel = DatabaseModel.FromRequest(request);
        LanguageCode defaultLanguageName = _translationConfiguration.SlugDefaultLanguage;
        string flowerName = request.FlowerNames
                                .Where(fn => fn.LanguageCode == defaultLanguageName)
                                .AsValueEnumerable()
                                .Select(fn => fn.Name)
                                .FirstOrDefault() ??
                            throw new DefaultLanguageTranslationMissingException(_translationConfiguration
                                .SlugDefaultLanguage);

        string slug = flowerName.GenerateSlug(_translationConfiguration.SlugDefaultLanguage);

        DatabaseModel dbModel = new DatabaseModel(
            Price: request.Price,
            Description: request.Description,
            Slug: slug,
            FlowerNames: request.FlowerNames);
        await _query.CreateFlower(dbModel, cancellationToken);

        return slug;
    }
}