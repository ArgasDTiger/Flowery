using Flowery.WebApi.Entities.Abstractions;

namespace Flowery.WebApi.Entities;

public sealed class Flower : IDeletable
{
    public int Id { get; init; }
    public string Slug { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public decimal Price { get; set; }
    public FlowerName FlowerName { get; set; } = null!;
    public string Description { get; set; } = null!;
}