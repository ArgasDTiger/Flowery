using System.IO.Pipelines;
using Flowery.Infrastructure.Images;
using Flowery.Infrastructure.Jobs;
using Flowery.Infrastructure.Jobs.Images;
using SixLabors.ImageSharp;

namespace Flowery.WebApi.Features.Flowers.Jobs;

public sealed class ProcessFlowerImagesJob : IJobExecutor<ProcessFlowerImages>
{
    private readonly IImageProcessor _imageProcessor;
    private readonly IImageSaver _imageSaver;
    private readonly IImageRetrieval _imageRetrieval;
    private readonly ILogger<ProcessFlowerImagesJob> _logger;
    private static readonly Size ThumbnailDimensions = new(400, 400);

    public ProcessFlowerImagesJob(
        IImageProcessor imageProcessor,
        IImageSaver imageSaver,
        IImageRetrieval imageRetrieval,
        ILogger<ProcessFlowerImagesJob> logger)
    {
        _imageProcessor = imageProcessor;
        _imageSaver = imageSaver;
        _imageRetrieval = imageRetrieval;
        _logger = logger;
    }

    public async Task Execute(ProcessFlowerImages payload)
    {
        await using var originalImageStream = _imageRetrieval.GetImageStream(payload.OriginalImagePath);

        string fileName = Path.GetFileNameWithoutExtension(payload.OriginalImagePath);
        await CreateThumbnail(originalImageStream, fileName + "-thumb.webp", payload.FolderName);
        // TODO: not com
        originalImageStream.Position = 0;
        await Compress(originalImageStream, fileName + "-com.webp", payload.FolderName);
        // await Task.WhenAll(thumbnailTask, compressTask);
    }

    private async Task CreateThumbnail(Stream imageStream, string fileName, string folderName)
    {
        var pipe = new Pipe();
        var writerTask = ThumbnailWriterTask(imageStream, pipe.Writer);

        try
        {
            await using var pipeReaderStream = pipe.Reader.AsStream();
            var saveTask = _imageSaver.SaveAsync(pipeReaderStream, folderName, fileName, CancellationToken.None);
            await Task.WhenAll(writerTask, saveTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An error occured while converting or saving the image of the flower to the thumbnail.");
            throw;
        }
    }

    private Task ThumbnailWriterTask(Stream stream, PipeWriter pipeWriter)
    {
        return Task.Run(async () =>
        {
            try
            {
                await using var pipeWriterStream = pipeWriter.AsStream();
                await _imageProcessor.ConvertToThumbnailAsync(stream, pipeWriterStream, ThumbnailDimensions,
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "An error occured while converting the image of the flower to the thumbnail in Writer.");
                await pipeWriter.CompleteAsync(ex);
                throw;
            }
            finally
            {
                await pipeWriter.CompleteAsync();
            }
        });
    }

    private async Task Compress(Stream imageStream, string fileName, string folderName)
    {
        var pipe = new Pipe();
        var writerTask = CompressWriterTask(imageStream, pipe.Writer);

        try
        {
            await using var pipeReaderStream = pipe.Reader.AsStream();
            var saveTask = _imageSaver.SaveAsync(pipeReaderStream, folderName, fileName, CancellationToken.None);
            await Task.WhenAll(writerTask, saveTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while compressing the image of the flower.");
            throw;
        }
    }

    private Task CompressWriterTask(Stream stream, PipeWriter pipeWriter)
    {
        return Task.Run(async () =>
        {
            try
            {
                await using var pipeWriterStream = pipeWriter.AsStream();
                await _imageProcessor.CompressAsync(stream, pipeWriterStream, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while compressing the image of the flower in Writer.");
                await pipeWriter.CompleteAsync(ex);
                throw;
            }
            finally
            {
                await pipeWriter.CompleteAsync();
            }
        });
    }
}