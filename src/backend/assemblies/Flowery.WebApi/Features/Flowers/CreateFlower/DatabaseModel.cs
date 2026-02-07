namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed record DatabaseModel(
    decimal Price,
    string Description,
    string Slug,
    ImmutableArray<FlowerNameRequest> FlowerNames);