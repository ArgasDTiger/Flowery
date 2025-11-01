using Flowery.WebApi.Shared.Models;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public interface IQuery
{
    Task<Response?> GetFlowerById(SlugOrId id, CancellationToken cancellationToken);
}