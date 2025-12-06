namespace Flowery.WebApi.Shared.Services;

internal sealed class TimeService : ITimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}