using Flowery.Shared.Enums;

namespace Flowery.WebApi.Features.Categories.CreateCategory;

public sealed record Request(ImmutableArray<CategoryNameRequest> CategoryNames);

public sealed record CategoryNameRequest(string Name, LanguageCode LanguageCode);
