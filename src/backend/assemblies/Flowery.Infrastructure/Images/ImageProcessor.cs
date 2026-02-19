using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Flowery.Infrastructure.Images;

internal sealed class ImageProcessor : IImageProcessor
{
    private static readonly string BaseDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

    public async Task<string> SaveOriginal(Stream stream, string name, string extension, string path,
        CancellationToken cancellationToken)
    {
        const string originalsFolder = "originals";

        extension = TrimDotFromExtension(extension);
        string fileName = $"{name}.{extension}";
        string fullPath = Path.Combine(BaseDir, path, originalsFolder, fileName);

        string directoryName = Path.GetDirectoryName(fullPath) ??
                               throw new InvalidOperationException("Could not create directory for image.");
        Directory.CreateDirectory(directoryName);

        await using var fileStream = new FileStream(fullPath, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken);
        return $"/images/{path}/{originalsFolder}/{fileName}";
    }

    public async Task<string> SaveThumbnail(Stream stream, string name, string path, Size dimensions,
        CancellationToken cancellationToken)
    {
        const string thumbFolder = "thumbnails";

        string thumbName = CreateWebpFile(name);
        string thumbDirectory = Path.Combine(BaseDir, path, thumbFolder);

        Directory.CreateDirectory(thumbDirectory);

        using var image = await Image.LoadAsync(stream, cancellationToken);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = dimensions,
            Mode = ResizeMode.Crop
        }));
        string pathToSave = Path.Combine(thumbDirectory, thumbName);
        await image.SaveAsWebpAsync(pathToSave,
            new WebpEncoder { Quality = 75 }, cancellationToken);
        return $"/images/{path}/{thumbFolder}/{thumbName}";
    }

    public async Task<string> SaveCompressed(Stream stream, string name, string path,
        CancellationToken cancellationToken)
    {
        const string compressedFolder = "compressed";

        string compressedFileName = CreateWebpFile(name);
        string compressedDirectory = Path.Combine(BaseDir, path, compressedFolder);

        Directory.CreateDirectory(compressedDirectory);

        using var image = await Image.LoadAsync(stream, cancellationToken);
        string pathToSave = Path.Combine(compressedDirectory, compressedFileName);
        await image.SaveAsWebpAsync(pathToSave,
            new WebpEncoder { Quality = 75 }, cancellationToken);
        return $"/images/{path}/{compressedFolder}/{compressedFileName}";
    }

    private static string CreateWebpFile(string fileName) => $"{fileName}.webp";
    
    private static string TrimDotFromExtension(string extension)
    {
        ReadOnlySpan<char> extenstionSpan = extension.AsSpan();
        if (extenstionSpan.Length > 0 && extenstionSpan[0] == '.')
        {
            extenstionSpan = extenstionSpan[1..]; 
            return extenstionSpan.ToString();
        }
        return extension;
    }
}