using Dapper;
using Flowery.WebApi.Infrastructure.Data;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> DeleteFlower(Guid id, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.ExecuteAsync(DeleteFlowerSql, new { Id = id });
    }

    private const string DeleteFlowerSql = """
                                           UPDATE flowers
                                           SET isdeleted = true,
                                           deletedatutc = now() at time zone 'utc',
                                           slug = null
                                           WHERE id = @Id
                                           """;
}