using Dapper;
using Flowery.Infrastructure.Data;

namespace Flowery.WebApi.Features.Categories.GetCategoryById;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Response?> GetCategoryBySlug(string slug, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var response = await connection.QuerySingleOrDefaultAsync<Response>(
            GetCategoryBySlugSql, new
            {
                Slug = slug
            });

        return response;
    }

    private const string GetCategoryBySlugSql =
        """
        SELECT c.Slug, cn.Name
        FROM Categories c
        JOIN CategoryName cn ON cn.CategoryId = c.Id
        WHERE c.Slug = @Slug
        """;
}