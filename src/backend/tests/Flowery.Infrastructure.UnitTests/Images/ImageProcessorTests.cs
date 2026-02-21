using SixLabors.ImageSharp;
using Flowery.Infrastructure.Images;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Metadata;

namespace Flowery.Infrastructure.UnitTests.Images;

public sealed class ImageProcessorTests
{
    [Fact]
    public async Task SaveOriginal_ShouldReturnExpectedPath()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var image = Substitute.For<Image>(new Configuration(), new PixelTypeInfo(0), new ImageMetadata(), new Size());
        image.SaveAsync(Arg.Any<string>(), cts.Token).Returns(Task.CompletedTask);

        var imageProcessor = new ImageProcessor();
        // Act

        string path = await imageProcessor.SaveOriginal(image, "name", "extension", "path", cts.Token);
        // Assert
        
        path.ShouldBe("/images/path/originals/name.extension");
    }
}