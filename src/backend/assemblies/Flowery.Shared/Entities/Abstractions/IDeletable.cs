namespace Flowery.Shared.Entities.Abstractions;

public interface IDeletable
{
    bool IsDeleted { get; init; }
    DateTimeOffset? DeletedAtUtc { get; init; }
}