using Dapper;
using Flowery.WebApi.Infrastructure.Data;
using Npgsql;
using NpgsqlTypes;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Guid> CreateFlower(DatabaseModel model, CancellationToken cancellationToken)
    {
        NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            Guid newFlowerId = await connection.QuerySingleAsync<Guid>(
                InsertFlowerSql,
                model,
                transaction);

            if (model.FlowerNames.Length > 0)
            {
                await using var importer =
                    await connection.BeginBinaryImportAsync(CopyCommand, cancellationToken);
                foreach (var flowerName in model.FlowerNames)
                {
                    await importer.StartRowAsync(cancellationToken);

                    await importer.WriteAsync(newFlowerId, NpgsqlDbType.Uuid, cancellationToken);
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
    
    private const string InsertFlowerSql = """
                                           INSERT INTO flowers (price, slug, description)
                                           VALUES (@Price, @Slug, @Description)
                                           RETURNING id;
                                           """;

    private const string CopyCommand = """
                                       COPY flowername (flowerid, languagecode, name) 
                                       FROM STDIN (FORMAT BINARY)
                                       """;
}