namespace Flowery.WebApi.Features.Categories.GetCategoryById;

public interface IQuery
{
    Task<Response?> GetCategoryBySlug(string slug, CancellationToken cancellationToken);
}