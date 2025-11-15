using Dapper;
using Flowery.WebApi.Infrastructure.Data;
using Flowery.WebApi.Shared.ActionResults.Static;
using Flowery.WebApi.Shared.Models;
using Npgsql;
using NpgsqlTypes;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<SlugWithId?> GetFlowerById(Guid id, CancellationToken cancellationToken)
    {
        NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<SlugWithId?>(GetFlowerByIdSql, new { Id = id });
    }
    
    public async Task<SlugWithId?> GetFlowerBySlug(string slug, CancellationToken cancellationToken)
    {
        NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<SlugWithId?>(GetFlowerBySlugSql, new { Slug = slug });
    }

    public async Task<OneOf<Success, NotFound>> UpdateFlower(DatabaseModel model, CancellationToken cancellationToken)
    {
        NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await connection.ExecuteAsync(UpdateFlowerSql, model, transaction: transaction);
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
            return new Success();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private const string GetFlowerByIdSql = """
                                            SELECT id, slug FROM flowers 
                                            WHERE id = @Id AND isdeleted = false;
                                            """;

    private const string GetFlowerBySlugSql = """
                                              SELECT id, slug FROM flowers 
                                              WHERE slug = @Slug AND isdeleted = false;
                                              """;

    private const string UpdateFlowerSql = """
                                           UPDATE flowers
                                           SET price = @Price, 
                                               slug = @Slug,
                                               description = @Description
                                           WHERE id = @Id AND isdeleted = false;
                                           """;

    private const string DeleteFlowerNamesSql = """
                                                DELETE FROM flowername
                                                WHERE flowerid = @FlowerId;
                                                """;

    private const string CopyCommand = """
                                       COPY flowername (flowerid, languagecode, name) 
                                       FROM STDIN (FORMAT BINARY)
                                       """;
}