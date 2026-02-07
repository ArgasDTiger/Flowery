using Dapper;
using Flowery.Infrastructure.Data;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Response?> GetFlowerBySlug(string slug, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.QuerySingleOrDefaultAsync<Response>(GetFlowerBySlugSql, new { Slug = slug });
    }

    private const string GetFlowerBySlugSql = """
                                              SELECT fn.name, f.slug, f.price
                                              FROM flowers f
                                              JOIN flowername fn ON f.id = fn.flowerid
                                              WHERE slug = @Slug AND isdeleted = false
                                              LIMIT 1;
                                              """;
}