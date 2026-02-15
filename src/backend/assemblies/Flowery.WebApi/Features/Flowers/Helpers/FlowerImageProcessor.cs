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
        CancellationToken cancellationToken)
    {
        stream.Position = 0;
        string originalPath =
            await _imageProcessor.SaveOriginal(stream, imageName, extension, ImagesDir, cancellationToken);

        stream.Position = 0;
        string thumbnailPath =
            await _imageProcessor.SaveThumbnail(stream, imageName, ImagesDir, ThumbnailDimensions, cancellationToken);

        stream.Position = 0;
        string compressedPath = await _imageProcessor.SaveCompressed(stream, imageName, ImagesDir, cancellationToken);

        return new FlowerImageProcessorResponse(originalPath, thumbnailPath, compressedPath);
    }
}