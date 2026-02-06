using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PaginatedResponse<Response>> GetFlowers(Request paginationParams,
        CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var orderBy = paginationParams.SortBy switch
        {
            SortField.Name => "fn.Name",
            SortField.Price => "f.Price",
            _ => "fn.Name"
        };

        var orderDirection = paginationParams.SortDirection.ToSqlOrderDirection();
        var offset = paginationParams.GetSqlOffset();

        var queryResult = await dbConnection.QueryMultipleAsync(
            $"{GetFlowersSql(orderBy, orderDirection)}; {GetFlowersCountSql}", new
            {
                Offset = offset,
                PageSize = paginationParams.PageSize
            });

        var flowers = await queryResult.ReadAsync<Response>();
        var count = await queryResult.ReadSingleAsync<int>();

        return new PaginatedResponse<Response>
        {
            Items = [..flowers],
            TotalCount = count
        };
    }

    private static string GetFlowersSql(string orderBy, string orderDirection) => $"""
         SELECT
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

    private const string GetFlowersCountSql = "SELECT COUNT(*) FROM flowers WHERE isdeleted = false";
}