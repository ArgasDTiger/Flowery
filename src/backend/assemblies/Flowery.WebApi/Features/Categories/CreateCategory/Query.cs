using Dapper;
using Flowery.Infrastructure.Data;
using Npgsql;
using NpgsqlTypes;

namespace Flowery.WebApi.Features.Categories.CreateCategory;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task CreateCategory(DatabaseModel model, CancellationToken cancellationToken)
    {
        await using var connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            Guid newCategoryId =
                await connection.QuerySingleAsync<Guid>(InsertCategorySql, new { Slug = model.Slug }, transaction);

            await using (var importer = await connection.BeginBinaryImportAsync(CopyCommand, cancellationToken))
            {
                foreach (var categoryName in model.CategoryNames)
                {
                    await importer.StartRowAsync(cancellationToken);

                    await importer.WriteAsync(newCategoryId, NpgsqlDbType.Uuid, cancellationToken);
                    await importer.WriteAsync(categoryName.LanguageCode.ToString(), NpgsqlDbType.Text,
                        cancellationToken);
                    await importer.WriteAsync(categoryName.Name, NpgsqlDbType.Text, cancellationToken);
                }

                await importer.CompleteAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> CategoryExists(string name, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.ExecuteScalarAsync<bool>(CategoryExistsSql, new { Name = name });
    }

    private const string InsertCategorySql =
        """
        INSERT INTO Categories (Slug) VALUES (@Slug)
        RETURNING Id;
        """;

    private const string CopyCommand =
        "COPY CategoryName (CategoryId, LanguageCode, Name) FROM STDIN (FORMAT BINARY);";

    private const string CategoryExistsSql =
        """
        SELECT EXISTS (
                    SELECT 1
                    FROM CategoryName
                    WHERE Name = @Name)
        """;
}