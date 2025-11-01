namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly ILogger<Handler> _logger;

    public Handler(IQuery query, ILogger<Handler> logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<OneOf<Success, NotFound>> DeleteFlower(Guid id, CancellationToken cancellationToken)
    {
        int rowsAffected = await _query.DeleteFlower(id, cancellationToken);
        if (rowsAffected == 0)
        {
            _logger.LogDebug(
                "No rows were affected when trying to delete flower with id {flowerId}, returning an error", id);
            return new NotFound();
        }

        return new Success();
    }
}