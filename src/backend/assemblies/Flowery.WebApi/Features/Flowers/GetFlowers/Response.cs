namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed record Response(
    string Name,
    string Slug,
    decimal Price,
    ImmutableArray<CategoryResponse> Categories,
    string ThumbnailUrl);

public sealed record CategoryResponse(string Name, string Slug);