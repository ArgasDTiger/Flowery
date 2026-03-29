namespace Flowery.Infrastructure.Images;

public interface IImageSaver
{
    Task<string> SaveAsync(Stream fileStream, string folderName, string fileName, CancellationToken cancellationToken);
}