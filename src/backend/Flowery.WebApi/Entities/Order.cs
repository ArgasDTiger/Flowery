namespace Flowery.WebApi.Entities;

public sealed class Order
{
    public Guid Id { get; init; }
    public decimal TotalPrice { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Flower> Flowers { get; set; } = [];
}