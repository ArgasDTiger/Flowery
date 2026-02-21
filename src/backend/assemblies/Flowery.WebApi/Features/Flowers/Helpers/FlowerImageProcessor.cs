using Flowery.Infrastructure.Images;
using SixLabors.ImageSharp;

namespace Flowery.WebApi.Features.Flowers.Helpers;

public sealed class FlowerImageProcessor : IFlowerImageProcessor
{
    private readonly IImageProcessor _imageProcessor;

    private static readonly Size ThumbnailDimensions = new(400, 400);
    private const string ImagesDir = "flowers";

    public FlowerImageProcessor(IImageProcessor imageProcessor)
    {
        _imageProcessor = imageProcessor;
    }

    public async Task<FlowerImageProcessorResponse> ProcessImage(Stream stream, string imageName, string extension,
        bool includeThumbnail, CancellationToken cancellationToken)
    {
        using var image = await Image.LoadAsync(stream, cancellationToken);

        var saveOriginal = _imageProcessor.SaveOriginal(image, imageName, extension, ImagesDir, cancellationToken);
        var saveCompressed = _imageProcessor.SaveCompressed(image, imageName, ImagesDir, cancellationToken);

        if (includeThumbnail)
        {
            var saveThumbnail = _imageProcessor.SaveThumbnail(image, imageName, ImagesDir, ThumbnailDimensions, cancellationToken);
            var results = await Task.WhenAll(saveOriginal, saveCompressed, saveThumbnail);
            return new FlowerImageProcessorResponse(results[0], results[2], results[1]);
        }

        var paths = await Task.WhenAll(saveOriginal, saveCompressed);
        return new FlowerImageProcessorResponse(paths[0], null, paths[1]);
    }
}