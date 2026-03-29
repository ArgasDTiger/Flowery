namespace Flowery.Infrastructure.Images;

public interface IImageRetrieval
{
    Stream GetImageStream(string path);
}