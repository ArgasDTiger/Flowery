using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<OneOf<Success, NotFound>> DeleteFlowerBySlug(string slug, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.ExecuteAsync(DeleteFlowerBySlugSql, new { slug }) > 0
            ? StaticResults.Success
            : StaticResults.NotFound;
    }

    // TODO: leave only deletedatuc
    private const string DeleteFlowerBySlugSql = """
                                                 UPDATE flowers
                                                 SET isdeleted = true,
                                                 deletedatutc = now() at time zone 'utc',
                                                 slug = null
                                                 WHERE slug = @slug
                                                 """;
}