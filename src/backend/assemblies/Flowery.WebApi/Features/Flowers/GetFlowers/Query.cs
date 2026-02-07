using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Pagination;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly TranslationConfiguration _translationSettings;

    public Query(IDbConnectionFactory dbConnectionFactory, IOptions<TranslationConfiguration> translationSettings)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _translationSettings = translationSettings.Value;
    }

    public async Task<PaginatedResponse<Response>> GetFlowers(Request request, LanguageCode languageCode,
        CancellationToken cancellationToken)
    {
        await using var dbConnection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        (GetFlowersResponse[] flowersQueryResult, int totalCount) =
            await QueryFlowers(request, languageCode, dbConnection);

        Guid[] flowerIds = flowersQueryResult.AsValueEnumerable().Select(f => f.Id).ToArray();
        var categories = (await dbConnection.QueryAsync<GetCategoriesResponse>(GetCategoriesForFlowerSql, new
        {
            FlowerIds = flowerIds,
            DefaultLanguageCode = _translationSettings.SlugDefaultLanguageString,
            LanguageCode = languageCode.ToString()
        })).ToArray();

        var flowers = flowersQueryResult.AsValueEnumerable().Select(f => new Response(
            Name: f.Name,
            Slug: f.Slug,
            Price: f.Price,
            Categories: categories.AsValueEnumerable()
                .Where(c => c.FlowerId == f.Id)
                .Select(c => new CategoryResponse(c.CategoryName, c.CategorySlug))
                .ToImmutableArray())).ToImmutableArray();

        return new PaginatedResponse<Response>
        {
            Items = flowers,
            TotalCount = totalCount
        };
    }

    private async Task<(GetFlowersResponse[] Flowers, int TotalCount)> QueryFlowers(Request request,
        LanguageCode languageCode, NpgsqlConnection connection)
    {
        string orderBy = request.SortBy switch
        {
            SortField.Name => "fn.Name",
            SortField.Price => "f.Price",
            _ => "fn.Name"
        };

        string orderDirection = request.SortDirection.ToSqlOrderDirection();
        int offset = request.GetSqlOffset();
        bool searchByCategory = request.Category is not null;

        var queryResult = await connection.QueryMultipleAsync(GetFlowersSql(orderBy, orderDirection, searchByCategory),
            new
            {
                LanguageCode = languageCode.ToString(),
                DefaultLanguageCode = _translationSettings.SlugDefaultLanguageString,
                Category = request.Category,
                Offset = offset,
                PageSize = request.PageSize
            });

        var flowers = await queryResult.ReadAsync<GetFlowersResponse>();
        var count = await queryResult.ReadSingleAsync<int>();
        return (flowers.ToArray(), count);
    }

    private sealed record GetFlowersResponse(Guid Id, string Name, string Slug, decimal Price);

    private sealed record GetCategoriesResponse(Guid FlowerId, string CategoryName, string CategorySlug);

    private static string GetFlowersSql(string orderBy, string orderDirection, bool searchByCategory) =>
        $"""
         SELECT
             f.Id as Id,
             COALESCE(fn.Name, fn_default.Name) as Name,
             f.Slug,
             f.Price
         FROM Flowers f
         LEFT JOIN FlowerName fn ON fn.FlowerId = f.Id AND fn.LanguageCode = @LanguageCode::LanguageCode
         LEFT JOIN FlowerName fn_default ON f.Id = fn_default.FlowerId AND fn_default.LanguageCode = @DefaultLanguageCode::LanguageCode
         {(searchByCategory ? """
                              JOIN FlowerCategory fc ON fc.FlowerId = f.Id
                              JOIN Categories c ON fc.CategoryId = c.Id
                              """ : "")}
         WHERE f.IsDeleted = false
         {(searchByCategory ? " AND c.Slug = @Category" : "")}
         ORDER BY {orderBy} {orderDirection}
         OFFSET @Offset
         LIMIT @PageSize;

         SELECT COUNT(*) FROM Flowers WHERE IsDeleted = false;
         """;

    private const string GetCategoriesForFlowerSql =
        """
        SELECT
            fc.FlowerId,
            COALESCE(cn.Name, cn_default.Name) as CategoryName,
            c.Slug as CategorySlug
        FROM FlowerCategory fc
        JOIN Categories c ON fc.CategoryId = c.Id
        JOIN CategoryName cn_default ON fc.CategoryId = cn_default.CategoryId AND cn_default.LanguageCode = @DefaultLanguageCode::LanguageCode
        LEFT JOIN CategoryName cn ON fc.CategoryId = cn.CategoryId 
            AND cn.LanguageCode = @LanguageCode::LanguageCode
        WHERE fc.FlowerId = ANY(@FlowerIds)
        """;
}