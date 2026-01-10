namespace Flowery.Domain.Entities;

public sealed class FlowerOrder
{
    public Guid FlowerId { get; init; }
    public Guid OrderId { get; init; }
    public int Quantity { get; init; }
}