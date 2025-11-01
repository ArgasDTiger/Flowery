using Dapper;
using Flowery.WebApi.Infrastructure.Data;
using Flowery.WebApi.Shared.Models;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Response?> GetFlowerById(SlugOrId id, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        if (id.Slug is null)
        {
            return await dbConnection.QuerySingleOrDefaultAsync<Response>(GetFlowerByIdSql, new { Id = id.Id });
        }
        return await dbConnection.QuerySingleOrDefaultAsync<Response>(GetFlowerBySlugSql, new { Slug = id.Slug });
    }

    private const string GetFlowerByIdSql = """
                                            SELECT f.id, fn.name, f.slug, f.price
                                            FROM flowers f
                                            JOIN flowername fn ON f.id = fn.flowerid
                                            WHERE id = @Id AND isdeleted = false
                                            LIMIT 1;
                                            """;

    private const string GetFlowerBySlugSql = """
                                            SELECT f.id, fn.name, f.slug, f.price
                                            FROM flowers f
                                            JOIN flowername fn ON f.id = fn.flowerid
                                            WHERE slug = @Slug AND isdeleted = false
                                            LIMIT 1;
                                            """;
}