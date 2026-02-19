using Flowery.Shared.Entities;
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

    public async Task<string> CreateFlower(HandlerModel request, CancellationToken cancellationToken)
    {
        var primaryImage = await ProcessImage(request.PrimaryImage, cancellationToken);
        var galleryImages = await ProcessGalleryImages(request.GalleryImages, cancellationToken);

        LanguageCode defaultLanguageName = _translationConfiguration.SlugDefaultLanguage;
        string flowerName = request.FlowerNames
                                .AsValueEnumerable()
                                .Where(fn => fn.LanguageCode == defaultLanguageName)
                                .Select(fn => fn.Name)
                                .FirstOrDefault() ?? throw new DefaultLanguageTranslationMissingException(_translationConfiguration.SlugDefaultLanguage);

        string slug = flowerName.GenerateSlug(_translationConfiguration.SlugDefaultLanguage);

        DatabaseModel dbModel = new DatabaseModel(
            Price: request.Price,
            Description: request.Description,
            Slug: slug,
            FlowerNames: request.FlowerNames);
        await _query.CreateFlower(dbModel, cancellationToken);

        return slug;
    }

    private async ValueTask<ImmutableArray<Image>> ProcessGalleryImages(ImmutableArray<ImageModel> images,
        CancellationToken cancellationToken)
    {
        if (images.IsDefaultOrEmpty) return ImmutableArray<Image>.Empty;
        if (images.Length == 1) return [ await ProcessImage(images[0], cancellationToken) ];
        var tasks = images.Select(async image => await ProcessImage(image, cancellationToken));
        Image[] results = await Task.WhenAll(tasks);
        return [.. results];
    }

    private async Task<Image> ProcessImage(ImageModel image, CancellationToken cancellationToken)
    {
        Guid imageId = Guid.CreateVersion7();
        string extension = Path.GetExtension(image.Extension);
        var processorResponse = await _imageProcessor.ProcessImage(image.ImageStream, imageId.ToString(), extension, cancellationToken);
        return new Image(Id: imageId,
            PathToSource: processorResponse.PrimaryImagePath,
            CompressedPath: processorResponse.CompressedImagePath,
            ThumbnailPath: processorResponse.ThumbnailImagePath);
    }
}