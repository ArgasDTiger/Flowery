using Flowery.WebApi.Features.Categories.CreateCategory;

namespace Flowery.WebApi.Features.Categories.UpdateCategory;

public sealed record DatabaseModel(
    Guid Id,
    string OldSlug,
    string? NewSlug,
    ImmutableArray<CategoryNameRequest> CategoryNames,
    bool NameChanged);