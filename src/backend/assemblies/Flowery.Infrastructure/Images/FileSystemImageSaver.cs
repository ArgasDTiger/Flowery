namespace Flowery.Infrastructure.Images;

internal sealed class FileSystemImageSaver : IImageSaver
{
    private static readonly string BaseDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
    private const int DefaultBufferSize = 8092;

    public async Task<string> SaveAsync(Stream fileStream, string folderName, string fileName,
        CancellationToken cancellationToken)
    {
        string directoryPath = Path.Combine(BaseDir, folderName);
        Directory.CreateDirectory(directoryPath);

        string fullPath = Path.Combine(directoryPath, fileName);

        await using var destinationStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None,
            DefaultBufferSize, FileOptions.Asynchronous);
        await fileStream.CopyToAsync(destinationStream, cancellationToken);

        return $"/images/{folderName}/{fileName}";
    }
}