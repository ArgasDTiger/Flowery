using Dapper;
using Flowery.WebApi.Infrastructure.Data;
using Flowery.WebApi.Shared.Extensions;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ImmutableArray<Response>> GetFlowers(Request paginationParams,
        CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var orderBy = paginationParams.SortField switch
        {
            SortField.Name => "fn.Name",
            SortField.Price => "f.Price",
            _ => "fn.Name"
        };

        var orderDirection = paginationParams.SortDirection.ToSqlOrderDirection();
        var offset = paginationParams.GetSqlOffset();

        string sql = $"""
                      SELECT
                          f.id,
                          fn.name,
                          f.slug,
                          f.price
                      FROM flowers f
                      JOIN flowername fn ON f.id = fn.flowerid
                      WHERE f.isdeleted = false
                      ORDER BY {orderBy} {orderDirection}
                      OFFSET @Offset ROWS
                      FETCH NEXT @PageSize ROWS ONLY
                      """;

        var flowers = await dbConnection.QueryAsync<Response>(sql, new
        {
            Offset = offset,
            PageSize = paginationParams.PageSize
        });

        return [..flowers];
    }
}