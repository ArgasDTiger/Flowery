namespace Flowery.Domain.Entities.Abstractions;

public interface IDeletable
{
    public bool IsDeleted { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
}