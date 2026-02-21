using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Categories.GetCategories;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly TranslationConfiguration _translationSettings;

    public Query(IDbConnectionFactory dbConnectionFactory, IOptions<TranslationConfiguration> translationSettings)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _translationSettings = translationSettings.Value;
    }

    public async Task<ImmutableArray<Response>> GetCategories(LanguageCode languageCode, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var categories = await connection.QueryAsync<Response>(GetCategoriesSql, new 
        { 
            LanguageCode = languageCode.ToString(),
            DefaultLanguageCode = _translationSettings.SlugDefaultLanguageString
        });
        return [..categories];
    }

    private const string GetCategoriesSql =
        $"""
         SELECT COALESCE(cn_requested.Name, cn_default.Name) as {nameof(Response.Name)}, Categories.{nameof(Response.Slug)}
         FROM Categories
         LEFT JOIN CategoryName cn_requested ON Categories.Id = cn_requested.Categoryid AND cn_requested.LanguageCode = @LanguageCode::LanguageCode
         LEFT JOIN CategoryName cn_default ON Categories.Id = cn_default.Categoryid AND cn_default.LanguageCode = @DefaultLanguageCode::LanguageCode
         WHERE cn_requested.Name IS NOT NULL OR cn_default.Name IS NOT NULL
         """;
}