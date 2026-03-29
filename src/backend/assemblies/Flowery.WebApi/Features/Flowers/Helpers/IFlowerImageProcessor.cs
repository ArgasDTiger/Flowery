namespace Flowery.WebApi.Features.Flowers.Helpers;

public interface IFlowerImageProcessor
{
    Task<string> SaveImage(Stream stream, string fileName, CancellationToken cancellationToken);
    void SaveCopies(Guid flowerId, string originalFilePath);
}