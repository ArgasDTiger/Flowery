namespace Flowery.WebApi.Shared.Models;

public readonly record struct SlugOrId(string? Slug, Guid? Id);