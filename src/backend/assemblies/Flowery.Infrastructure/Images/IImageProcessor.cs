using SixLabors.ImageSharp;

namespace Flowery.Infrastructure.Images;

public interface IImageProcessor
{
    Task<string> SaveOriginal(Stream stream, string name, string extension, string path,
        CancellationToken cancellationToken);

    Task<string> SaveOriginal(Image image, string name, string extension, string path,
        CancellationToken cancellationToken);

    Task<string> SaveThumbnail(Stream stream, string name, string path, Size dimensions,
        CancellationToken cancellationToken);

    Task<string> SaveThumbnail(Image image, string name, string path, Size dimensions,
        CancellationToken cancellationToken);

    Task<string> SaveCompressed(Stream stream, string name, string path,
        CancellationToken cancellationToken);

    Task<string> SaveCompressed(Image image, string name, string path,
        CancellationToken cancellationToken);
}