using Flowery.WebApi.Flowers;

namespace Flowery.WebApi.Orders;

public sealed class Order
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Flower> Flowers { get; set; } = [];
}