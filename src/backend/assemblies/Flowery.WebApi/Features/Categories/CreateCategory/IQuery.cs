namespace Flowery.WebApi.Features.Categories.CreateCategory;

public interface IQuery
{
    Task CreateCategory(DatabaseModel model, CancellationToken cancellationToken);
    Task<bool> CategoryExists(string name, CancellationToken cancellationToken);
}