using Azure.Storage.Blobs;

namespace Flowery.Infrastructure.Images;

internal sealed class AzureImageRetrieval(BlobServiceClient client) : IImageRetrieval
{
    private const string ContainerName = "images";

    public Task<Stream> GetImageStream(string path)
    {
        path = path.TrimStart('/');
        
        var containerClient = client.GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(path);

        return blobClient.OpenReadAsync();
    }
}