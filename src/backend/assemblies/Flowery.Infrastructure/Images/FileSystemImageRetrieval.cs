namespace Flowery.Infrastructure.Images;

internal sealed class FileSystemImageRetrieval : IImageRetrieval
{
    // TODO: probably concat with saver
    private static readonly string BaseDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    private const int DefaultBufferSize = 4096;

    public Task<Stream> GetImageStream(string path)
    {
        // TODO: remove slash from saving in db? concat logic with saver? spans?
        path = path.TrimStart('/');
        path = Path.Combine(BaseDir, path);
        Stream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize,
            FileOptions.Asynchronous);
        return Task.FromResult(fileStream);
    }
}