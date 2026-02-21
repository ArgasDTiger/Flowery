using Flowery.Shared.Entities;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed record DatabaseModel(
    Guid Id,
    decimal Price,
    string Description,
    string Slug,
    ImmutableArray<FlowerNameRequest> FlowerNames,
    Image PrimaryImage,
    ImmutableArray<Image> GalleryImages);