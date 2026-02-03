namespace Flowery.Shared.Services;

public interface ITimeService
{
    DateTimeOffset UtcNowOffset { get; }
    DateTime UtcNow { get; }
}