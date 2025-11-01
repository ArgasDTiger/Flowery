using Dapper;
using Flowery.WebApi.Infrastructure.Data;
using Npgsql;
using NpgsqlTypes;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    private const string InsertFlowerSql = """
                                           INSERT INTO flowers (price, slug)
                                           VALUES (@Price, @Slug)
                                           RETURNING id;
                                           """;

    private const string CopyCommand = """
                                       COPY flowername (flowerid, languagecode, name) 
                                       FROM STDIN (FORMAT BINARY)
                                       """;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> CreateFlower(DatabaseModel model, CancellationToken cancellationToken)
    {
        NpgsqlConnection npgsqlConnection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        await using var connection = npgsqlConnection;
        await using var transaction = await npgsqlConnection.BeginTransactionAsync(cancellationToken);
        try
        {
            int newFlowerId = await npgsqlConnection.QuerySingleAsync<int>(
                InsertFlowerSql,
                new { model.Price, model.Slug },
                transaction);

            if (model.FlowerNames.Count > 0)
            {
                await using var importer =
                    await npgsqlConnection.BeginBinaryImportAsync(CopyCommand, cancellationToken);
                foreach (var flowerName in model.FlowerNames)
                {
                    await importer.StartRowAsync(cancellationToken);

                    await importer.WriteAsync(newFlowerId, NpgsqlDbType.Integer, cancellationToken);
                    await importer.WriteAsync(flowerName.LanguageCode.ToString(), NpgsqlDbType.Text,
                        cancellationToken);
                    await importer.WriteAsync(flowerName.Name, NpgsqlDbType.Text, cancellationToken);
                }

                await importer.CompleteAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return newFlowerId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}