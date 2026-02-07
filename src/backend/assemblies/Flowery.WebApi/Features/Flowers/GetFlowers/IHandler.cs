using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public interface IHandler
{
    Task<PaginatedResponse<Response>> GetFlowers(Request request, LanguageCode languageCode, CancellationToken cancellationToken);
}