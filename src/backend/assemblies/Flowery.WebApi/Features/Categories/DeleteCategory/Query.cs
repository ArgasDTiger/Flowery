using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Npgsql;

namespace Flowery.WebApi.Features.Categories.DeleteCategory;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Guid?> GetCategoryIdBySlug(string slug, CancellationToken cancellationToken)
    {
        await using var connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Guid?>(GetCategoryBySlugSql, new { Slug = slug });
    }

    public async Task<OneOf<Success, NotFound>> DeleteCategoryById(Guid categoryId, CancellationToken cancellationToken)
    {
        await using var connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        int rowsAffected = await connection.ExecuteAsync(DeleteCategoryByIdSql, new { CategoryId = categoryId });
        return rowsAffected > 0
            ? StaticResults.Success
            : throw new Exception($"Failed to delete category {categoryId}.");
    }

    private const string GetCategoryBySlugSql = "SELECT Id FROM Categories WHERE Slug = @Slug LIMIT 1";

    private const string DeleteCategoryByIdSql =
        """
        WITH deleted_names AS (
            DELETE FROM CategoryName WHERE CategoryId = @CategoryId
        ),
        deleted_flower_categories AS (
            DELETE FROM FlowerCategory WHERE CategoryId = @CategoryId
        )
        DELETE FROM Categories WHERE Id = @CategoryId;
        """;
}