namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed record Response(
    string Name,
    string Slug,
    decimal Price,
    string PrimaryImageUrl,
    string[] GalleryImageUrls);