using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Flowery.Infrastructure.Images;

internal sealed class ImageProcessor : IImageProcessor
{
    private static readonly WebpEncoder WebpEncoder = new() { Quality = 75 };

    public async Task ConvertToThumbnailAsync(Stream sourceStream, Stream destinationStream, Size dimensions,
        CancellationToken cancellationToken)
    {
        using var image = await Image.LoadAsync(sourceStream, cancellationToken);
        using var clone = image.Clone(x => x.Resize(new ResizeOptions
        {
            Size = dimensions,
            Mode = ResizeMode.Crop
        }));

        await clone.SaveAsWebpAsync(destinationStream, WebpEncoder, cancellationToken);
    }

    public async Task CompressAsync(Stream sourceStream, Stream destinationStream,
        CancellationToken cancellationToken)
    {
        using var image = await Image.LoadAsync(sourceStream, cancellationToken);
        await image.SaveAsWebpAsync(destinationStream, WebpEncoder, cancellationToken);
    }
}