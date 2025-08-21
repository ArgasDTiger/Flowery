namespace Flowery.WebApi.Entities;

public sealed class UserFlower
{
    public int FlowerId { get; set; }
    public Guid UserId { get; set; }
}