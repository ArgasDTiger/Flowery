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

    public Handler(
        IQuery query,
        IOptions<TranslationConfiguration> translationSettings,
        IFlowerImageProcessor imageProcessor)
    {
        _query = query;
        _imageProcessor = imageProcessor;
        _translationConfiguration = translationSettings.Value;
    }

    public async Task<string> CreateFlower(HandlerModel request, CancellationToken cancellationToken)
    {
        var primaryImageTask = ProcessImage(request.PrimaryImage, cancellationToken);
        var galleryImagesTask = ProcessGalleryImages(request.GalleryImages, cancellationToken);

        LanguageCode defaultLanguageName = _translationConfiguration.SlugDefaultLanguage;
        string flowerName = request.FlowerNames
                                .AsValueEnumerable()
                                .Where(fn => fn.LanguageCode == defaultLanguageName)
                                .Select(fn => fn.Name)
                                .FirstOrDefault() ??
                            throw new DefaultLanguageTranslationMissingException(_translationConfiguration
                                .SlugDefaultLanguage);

        string slug = flowerName.GenerateSlug(_translationConfiguration.SlugDefaultLanguage);
        Guid flowerId = Guid.CreateVersion7();

        await Task.WhenAll(primaryImageTask, galleryImagesTask);

        DatabaseModel dbModel = new DatabaseModel(
            Id: flowerId,
            Price: request.Price,
            Description: request.Description,
            Slug: slug,
            FlowerNames: request.FlowerNames,
            PrimaryImage: await primaryImageTask,
            GalleryImages: await galleryImagesTask);
        await _query.CreateFlower(dbModel, cancellationToken);

        SaveCopies(await primaryImageTask, await galleryImagesTask);

        // TODO: job to remove file?
        return slug;
    }

    private void SaveCopies(Image primaryImage, ImmutableArray<Image> galleryImages)
    {
        _imageProcessor.SaveCopies(primaryImage.Id, primaryImage.PathToSource);
        foreach (var image in galleryImages)
        {
            _imageProcessor.SaveCopies(image.Id, image.PathToSource);
        }
    }

    private async Task<ImmutableArray<Image>> ProcessGalleryImages(ImmutableArray<ImageModel> images,
        CancellationToken cancellationToken)
    {
        if (images.IsDefaultOrEmpty) return ImmutableArray<Image>.Empty;
        if (images.Length == 1) return [await ProcessImage(images[0], cancellationToken)];

        var processingTasks = images.Select(image => ProcessImage(image, cancellationToken));
        Image[] results = await Task.WhenAll(processingTasks);
        return [.. results];
    }

    private async Task<Image> ProcessImage(ImageModel image, CancellationToken cancellationToken)
    {
        Guid imageId = Guid.CreateVersion7();
        string extension = Path.GetExtension(image.Extension);
        string fileName = $"{imageId}{extension}";
        var imagePath = await _imageProcessor.SaveImage(image.ImageStream, fileName, cancellationToken);
        return new Image(imageId, imagePath);
    }
}