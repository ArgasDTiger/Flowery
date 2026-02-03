namespace Flowery.Shared.Entities;

public sealed class UserFlower
{
    public Guid FlowerId { get; init; }
    public Guid UserId { get; init; }
}