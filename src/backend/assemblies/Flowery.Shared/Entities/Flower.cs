using Flowery.Shared.Entities.Abstractions;

namespace Flowery.Shared.Entities;

public sealed class Flower : IDeletable
{
    public Guid Id { get; init; }
    public string? Slug { get; init; }
    public bool IsDeleted { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
    public decimal Price { get; init; }
    public FlowerName FlowerName { get; init; } = null!;
    public string Description { get; init; } = null!;
    public Guid PrimaryImageId { get; init; }
}