namespace Flowery.Domain.Services;

public interface ITimeService
{
    DateTimeOffset UtcNowOffset { get; }
    DateTime UtcNow { get; }
}