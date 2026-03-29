using SixLabors.ImageSharp;

namespace Flowery.Infrastructure.Images;

public interface IImageProcessor
{
    Task ConvertToThumbnailAsync(Stream sourceStream, Stream destinationStream, Size dimensions, CancellationToken cancellationToken);
    Task CompressAsync(Stream sourceStream, Stream destinationStream, CancellationToken cancellationToken);
}