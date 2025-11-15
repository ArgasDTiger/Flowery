namespace Flowery.WebApi.Entities;

public sealed class Order
{
    public Guid Id { get; init; }
    public decimal TotalPrice { get; init; }
    public Guid UserId { get; init; }
    public ICollection<Flower> Flowers { get; init; } = [];
}