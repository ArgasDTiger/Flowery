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

    public async Task<OneOf<Success, Error>> DeleteFlowerById(Guid id, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.ExecuteAsync(DeleteFlowerByIdSql, new { id }) > 0
            ? StaticResults.Success
            : new Error("Flower is not found.");
    }

    public async Task<OneOf<Success, Error>> DeleteFlowerBySlug(string slug, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.ExecuteAsync(DeleteFlowerBySlugSql, new { slug }) > 0
            ? StaticResults.Success
            : new Error("Flower is not found.");
    }

    private const string DeleteFlowerByIdSql = """
                                               UPDATE flowers
                                               SET isdeleted = true,
                                               deletedatutc = now() at time zone 'utc',
                                               slug = null
                                               WHERE id = @id
                                               """;

    private const string DeleteFlowerBySlugSql = """
                                                 UPDATE flowers
                                                 SET isdeleted = true,
                                                 deletedatutc = now() at time zone 'utc',
                                                 slug = null
                                                 WHERE slug = @slug
                                                 """;
}