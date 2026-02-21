namespace Flowery.WebApi.Features.Categories.CreateCategory;

public sealed record DatabaseModel(
    string Slug,
    ImmutableArray<CategoryNameRequest> CategoryNames);