using Flowery.WebApi.Features.Flowers.Helpers;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed record HandlerModel(
    decimal Price,
    ImmutableArray<FlowerNameRequest> FlowerNames,
    string Description,
    ImageModel PrimaryImage,
    ImmutableArray<ImageModel> GalleryImages);