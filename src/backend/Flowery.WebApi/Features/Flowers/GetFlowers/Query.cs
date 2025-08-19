using Dapper;
using Flowery.WebApi.Infrastructure.Data;
using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<Response>> GetFlowers(Request paginationParams, CancellationToken cancellationToken)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
    
        var orderBy = paginationParams.SortField switch
        {
            SortField.Name => "fn.Name",
            SortField.Price => "f.Price",
            _ => "fn.Name"
        };
    
        var sortDirection = paginationParams.SortDirection == SortDirection.Descending ? "DESC" : "ASC";
    
        var offset = (paginationParams.PageNumber - 1) * paginationParams.PageSize;
    
        const string sql = """
                           SELECT 
                               f.Id,
                               fn.Name,
                               f.Slug,
                               f.Price
                           FROM Flowers f
                           JOIN FlowerName fn ON f.Id = fn.FlowerId
                           WHERE f.IsDeleted = 0
                           ORDER BY @OrderBy @OrderDirection
                           OFFSET @Offset ROWS
                           FETCH NEXT @PageSize ROWS ONLY
                           """;
    
        var flowers = await dbConnection.QueryAsync<Response>(sql, new
        {
            OrderBy = orderBy,
            OrderDirection = sortDirection,
            Offset = offset,
            PageSize = paginationParams.PageSize
        });
    
        return flowers.ToList();
    }
}