using FluentValidation.Results;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public interface IHandler
{
    Task<OneOf<List<Response>, List<ValidationFailure>>> GetFlowers(Request request,
        CancellationToken cancellationToken);
}