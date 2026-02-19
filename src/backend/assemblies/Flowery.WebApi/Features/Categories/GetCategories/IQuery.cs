using Flowery.Shared.Enums;

namespace Flowery.WebApi.Features.Categories.GetCategories;

public interface IQuery
{
    Task<ImmutableArray<Response>> GetCategories(LanguageCode languageCode, CancellationToken cancellationToken);
}