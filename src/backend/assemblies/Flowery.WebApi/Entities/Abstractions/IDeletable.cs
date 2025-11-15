namespace Flowery.WebApi.Entities.Abstractions;

public interface IDeletable
{
    public bool IsDeleted { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
}