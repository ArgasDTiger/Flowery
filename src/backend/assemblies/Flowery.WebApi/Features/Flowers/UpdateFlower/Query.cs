using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.WebApi.Shared.Configurations;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly TranslationConfiguration _translationSettings;

    public Query(IDbConnectionFactory dbConnectionFactory, IOptions<TranslationConfiguration> translationSettings)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _translationSettings = translationSettings.Value;
    }

    public async Task<FlowerBySlugModel?> GetFlowerIdBySlug(string slug, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<FlowerBySlugModel?>(GetFlowerIdBySlugSql, new
        {
            Slug = slug,
            NameLanguage = _translationSettings.SlugDefaultLanguage
        });
    }

    public async Task<OneOf<Success, NotFound>> UpdateFlower(DatabaseModel model, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await UpdateFlower(model, connection, transaction);

            await connection.ExecuteAsync(DeleteFlowerNamesSql, new { FlowerId = model.Id },
                transaction: transaction);

            await using var importer =
                await connection.BeginBinaryImportAsync(CopyCommand, cancellationToken);

            foreach (var flowerName in model.FlowerNames)
            {
                await importer.StartRowAsync(cancellationToken);
                await importer.WriteAsync(model.Id, NpgsqlDbType.Uuid, cancellationToken);
                await importer.WriteAsync(flowerName.LanguageCode.ToString(), NpgsqlDbType.Text, cancellationToken);
                await importer.WriteAsync(flowerName.Name, NpgsqlDbType.Text, cancellationToken);
            }

            await importer.CompleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return StaticResults.Success;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static async Task UpdateFlower(DatabaseModel model, NpgsqlConnection connection,
        NpgsqlTransaction transaction)
    {
        var parameters = new DynamicParameters();
        parameters.Add(nameof(model.Price), model.Price);
        parameters.Add(nameof(model.Description), model.Description);
        parameters.Add(nameof(model.OldSlug), model.OldSlug);

        string sqlQuery;
        if (model.NameChanged)
        {
            parameters.Add(nameof(model.NewSlug), model.NewSlug);
            sqlQuery = UpdateFlowerWithSlugSql;
        }
        else
        {
            sqlQuery = UpdateFlowerWithoutSlugSql;
        }

        await connection.ExecuteAsync(sqlQuery, model, transaction: transaction);
    }

    private const string GetFlowerIdBySlugSql =
        """
        SELECT f.Id, fn.Name FROM Flowers f 
        JOIN FlowerName fn ON fn.FlowerId = f.Id 
        WHERE f.Slug = @Slug 
          AND fn.LanguageCode = @NameLanguage 
          AND f.IsDeleted = false
        LIMIT 1
        """;

    private const string UpdateFlowerWithSlugSql =
        """
        UPDATE Flowers
        SET Price = @Price, 
            Slug = @NewSlug,
            Description = @Description
        WHERE Id = @Id AND IsDeleted = false;
        """;

    private const string UpdateFlowerWithoutSlugSql =
        """
        UPDATE Flowers
        SET Price = @Price, Description = @Description
        WHERE Id = @Id AND IsDeleted = false;
        """;

    private const string DeleteFlowerNamesSql =
        """
        DELETE FROM FlowerName
        WHERE FlowerId = @FlowerId;
        """;

    private const string CopyCommand =
        """
        COPY flowername (flowerid, languagecode, name) 
        FROM STDIN (FORMAT BINARY)
        """;
}