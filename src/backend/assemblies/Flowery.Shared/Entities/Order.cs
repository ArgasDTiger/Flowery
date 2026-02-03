using System.Collections.Immutable;

namespace Flowery.Shared.Entities;

public sealed class Order
{
    public Guid Id { get; init; }
    public decimal TotalPrice { get; init; }
    public Guid UserId { get; init; }
    public ImmutableList<Flower> Flowers { get; init; } = ImmutableList<Flower>.Empty;
}