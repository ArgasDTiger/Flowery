using Flowery.Shared.Enums;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public interface IQuery
{
    Task<Response?> GetFlowerBySlug(string slug, LanguageCode languageCode, CancellationToken cancellationToken);
}