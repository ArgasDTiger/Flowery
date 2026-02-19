using Flowery.Shared.Enums;

namespace Flowery.WebApi.Features.Categories.GetCategories;

public interface IHandler
{
    Task<ImmutableArray<Response>> GetCategories(LanguageCode languageCode, CancellationToken cancellationToken);
}