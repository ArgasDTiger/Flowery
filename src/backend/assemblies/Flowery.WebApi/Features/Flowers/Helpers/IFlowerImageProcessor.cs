namespace Flowery.WebApi.Features.Flowers.Helpers;

public interface IFlowerImageProcessor
{
    Task<FlowerImageProcessorResponse> ProcessImage(Stream stream, string imageName, string extension, bool includeThumbnail, CancellationToken cancellationToken);
}