using FluentValidation.Results;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public interface IHandler
{
    Task<OneOf<ImmutableArray<Response>, IReadOnlyList<ValidationFailure>>> GetFlowers(Request request,
        CancellationToken cancellationToken);
}