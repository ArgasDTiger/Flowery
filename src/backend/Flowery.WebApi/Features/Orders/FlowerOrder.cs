namespace Flowery.WebApi.Orders;

public sealed class FlowerOrder
{
    public int FlowerId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
}