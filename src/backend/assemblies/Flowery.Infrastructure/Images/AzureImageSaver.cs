namespace Flowery.Infrastructure.Images;

using Azure.Storage.Blobs;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

internal sealed class AzureImageSaver(BlobServiceClient client) : IImageSaver
{
    private const string ContainerName = "images";

    public async Task<string> SaveAsync(Stream fileStream, string folderName, string fileName, CancellationToken cancellationToken)
    {
        var containerClient = client.GetBlobContainerClient(ContainerName);
        
        string blobName = $"{folderName}/{fileName}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);

        return $"/{ContainerName}/{blobName}";
    }
}