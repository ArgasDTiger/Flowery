namespace Flowery.WebApi.Features.Flowers.Helpers;

public sealed record FlowerImageProcessorResponse(
    string PrimaryImagePath,
    string ThumbnailImagePath,
    string CompressedImagePath);