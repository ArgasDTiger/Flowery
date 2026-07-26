namespace Flowery.Infrastructure.Images;

public interface IImageRetrieval
{
    Task<Stream> GetImageStream(string path);
}