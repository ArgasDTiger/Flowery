namespace Flowery.WebApi.Shared.Services;

internal interface ITimeService
{
    DateTimeOffset UtcNowOffset { get; }
    DateTime UtcNow { get; }
}