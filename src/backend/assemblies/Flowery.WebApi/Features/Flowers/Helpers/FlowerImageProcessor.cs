using Flowery.Infrastructure.Images;
using Flowery.Infrastructure.Jobs;
using Flowery.Infrastructure.Jobs.Images;
using Hangfire;

namespace Flowery.WebApi.Features.Flowers.Helpers;

internal sealed class FlowerImageProcessor : IFlowerImageProcessor
{
    private readonly IImageSaver _imageSaver;
    private readonly IBackgroundJobClient _backgroundJobClient;

    private const string ImagesDir = "flowers";

    public FlowerImageProcessor(IImageSaver imageSaver, IBackgroundJobClient backgroundJobClient)
    {
        _imageSaver = imageSaver;
        _backgroundJobClient = backgroundJobClient;
    }

    public Task<string> SaveImage(Stream stream, string fileName, CancellationToken cancellationToken)
    {
        return _imageSaver.SaveAsync(stream, ImagesDir, fileName, cancellationToken);
    }

    public void SaveCopies(Guid flowerId, string originalFilePath)
    {
        var model = new ProcessFlowerImages(
            FlowerId: flowerId,
            FolderName: ImagesDir,
            OriginalImagePath: originalFilePath);
        _backgroundJobClient.Enqueue<IJobExecutor<ProcessFlowerImages>>(x => x.Execute(model));
    }
}