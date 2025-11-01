using FluentValidation.Results;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public interface IHandler
{
    Task<OneOf<Guid, IReadOnlyList<ValidationFailure>>> CreateFlower(Request request, CancellationToken cancellationToken);
}