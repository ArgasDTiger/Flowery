using Flowery.WebApi.Shared.Entities;

namespace Flowery.WebApi.Flowers;

public sealed class Flower : IDeletable
{
    public int Id { get; init; }
    public string Slug { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public decimal Price { get; set; }
    public FlowerName FlowerName { get; set; } = null!;
}