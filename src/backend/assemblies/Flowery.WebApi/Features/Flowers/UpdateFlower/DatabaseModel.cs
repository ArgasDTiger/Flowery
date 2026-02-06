namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed record DatabaseModel(
    Guid Id,
    string OldSlug,
    string? NewSlug,
    decimal Price,
    string Description,
    ImmutableArray<FlowerNameRequest> FlowerNames,
    bool NameChanged);