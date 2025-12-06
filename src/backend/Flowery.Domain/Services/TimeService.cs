namespace Flowery.Domain.Services;

internal sealed class TimeService : ITimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}