using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly TranslationConfiguration _translationSettings;

    public Query(IDbConnectionFactory dbConnectionFactory, IOptions<TranslationConfiguration> translationSettings)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _translationSettings = translationSettings.Value;
    }

    public async Task<Response?> GetFlowerBySlug(string slug, LanguageCode languageCode,
        CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.QuerySingleOrDefaultAsync<Response>(GetFlowerBySlugSql,
            new
            {
                Slug = slug,
                LanguageCode = languageCode,
                DefaultLanguageCode = _translationSettings.SlugDefaultLanguageString
            });
    }

    private const string GetFlowerBySlugSql =
        """
        SELECT COALESCE(fn_requested.Name, fn_default.Name) AS Name, f.Slug, f.Price
        FROM Flowers f
        LEFT JOIN FlowerName fn_requested ON f.Id = fn_requested.FlowerId AND fn_requested.LanguageCode = @LanguageCode
        LEFT JOIN FlowerName fn_default ON f.Id = fn_default.FlowerId AND fn_default.LanguageCode = @DefaultLanguageCode
        WHERE f.Slug = @Slug AND f.IsDeleted = false
        LIMIT 1;
        """;
}