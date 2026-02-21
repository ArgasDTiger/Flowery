using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.WebApi.Shared.Configurations;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Flowery.WebApi.Features.Categories.UpdateCategory;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly TranslationConfiguration _translationSettings;

    public Query(IDbConnectionFactory dbConnectionFactory, IOptions<TranslationConfiguration> translationSettings)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _translationSettings = translationSettings.Value;
    }

    public async Task<CategoryBySlugModel?> GetCategoryBySlug(string slug, CancellationToken cancellationToken)
    {
        await using var connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<CategoryBySlugModel?>(
            GetCategoryBySlugSql, new
            {
                Slug = slug, 
                NameLanguage = _translationSettings.SlugDefaultLanguageString
            });
    }

    public async Task<OneOf<Success, NotFound>> UpdateCategory(DatabaseModel model, CancellationToken cancellationToken)
    {
        await using var connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await UpdateCategorySlug(model, connection, transaction);
            await SaveCategoryNames(model, connection, transaction);
            await transaction.CommitAsync(cancellationToken);
            return StaticResults.Success;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static async Task UpdateCategorySlug(DatabaseModel model, NpgsqlConnection connection,
        NpgsqlTransaction transaction)
    {
        string sql = model.NameChanged ? UpdateCategoryWithSlugSql : UpdateCategoryWithoutSlugSql;
        await connection.ExecuteAsync(sql, model, transaction: transaction);
    }

    private static async Task SaveCategoryNames(DatabaseModel model, NpgsqlConnection connection,
        NpgsqlTransaction transaction)
    {
        var namesToUpsert = model.CategoryNames.Select(n => new
        {
            CategoryId = model.Id,
            LanguageCode = n.LanguageCode.ToString(),
            n.Name
        });

        await connection.ExecuteAsync(UpsertCategoryNamesSql, namesToUpsert, transaction: transaction);

        var notUsedLanguages = new
        {
            CategoryId = model.Id,
            LanguageCodes = model.CategoryNames.AsValueEnumerable().Select(n => n.LanguageCode.ToString()).ToArray()
        };

        await connection.ExecuteAsync(DeleteRemovedCategoryNamesSql, notUsedLanguages, transaction: transaction);
    }

    private const string GetCategoryBySlugSql =
        """
        SELECT c.Id, cn.Name FROM Categories c
        JOIN CategoryName cn ON cn.CategoryId = c.Id
        WHERE c.Slug = @Slug AND cn.LanguageCode = @NameLanguage::LanguageCode
        LIMIT 1
        """;

    private const string UpdateCategoryWithSlugSql = "UPDATE Categories SET Slug = @NewSlug WHERE Id = @Id";

    private const string UpdateCategoryWithoutSlugSql = "UPDATE Categories SET Slug = Slug WHERE Id = @Id";

    private const string UpsertCategoryNamesSql =
        """
        INSERT INTO CategoryName (CategoryId, LanguageCode, Name)
        VALUES (@CategoryId, @LanguageCode::LanguageCode, @Name)
        ON CONFLICT (CategoryId, LanguageCode)
        DO UPDATE SET Name = EXCLUDED.Name;
        """;

    private const string DeleteRemovedCategoryNamesSql = "DELETE FROM CategoryName WHERE CategoryId = @CategoryId AND LanguageCode != ALL(@LanguageCodes::LanguageCode[]);";
}