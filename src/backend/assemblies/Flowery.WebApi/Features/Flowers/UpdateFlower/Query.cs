using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Npgsql;
using NpgsqlTypes;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private const string Slug = "Slug";
    private const string NewSlug = "NewSlug";
    private const string OldSlug = "OldSlug";
    
    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> DoesFlowerExist(Guid id, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(FlowerByIdExistsSql, new { Id = id });
    }

    public async Task<bool> DoesFlowerExist(string slug, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(FLowerBySlugExistsSql, new { Slug = slug });
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

    private static async Task UpdateFlower(DatabaseModel model, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        var parameters = new DynamicParameters();
        parameters.Add(nameof(model.Price), model.Price);
        parameters.Add(nameof(model.Description), model.Description);
    
        string sql;
        if (model.IsIdGuid)
        {
            sql = model.IsSlugChanged 
                ? UpdateFlowerWithSlugByIdSql 
                : UpdateFlowerWithoutSlugByIdSql;
            parameters.Add(nameof(model.Id), model.Id);
            if (model.IsSlugChanged)
            {
                parameters.Add(Slug, model.NewSlug);
            }
        }
        else
        {
            sql = model.IsSlugChanged 
                ? UpdateFlowerWithSlugBySlugSql 
                : UpdateFlowerWithoutSlugBySlugSql;
            parameters.Add(model.IsSlugChanged ? NewSlug : OldSlug, model.IsSlugChanged ? model.NewSlug : model.OriginalSlug);
            parameters.Add(OldSlug, model.OriginalSlug);
        }
    
        await connection.ExecuteAsync(sql, parameters, transaction: transaction);
    }

    private const string FlowerByIdExistsSql = "SELECT EXISTS (SELECT 1 FROM Flowers WHERE id = @Id)";

    private const string FLowerBySlugExistsSql = "SELECT EXISTS (SELECT 1 FROM Flowers WHERE slug = @Slug)";

    private const string UpdateFlowerWithSlugByIdSql = """
                                                   UPDATE flowers
                                                   SET price = @Price, 
                                                       slug = @Slug,
                                                       description = @Description
                                                   WHERE id = @Id AND isdeleted = false;
                                                   """;

    private const string UpdateFlowerWithoutSlugByIdSql = """
                                                      UPDATE flowers
                                                      SET price = @Price, description = @Description
                                                      WHERE id = @Id AND isdeleted = false;
                                                      """;

    private const string UpdateFlowerWithSlugBySlugSql = """
                                                   UPDATE flowers
                                                   SET price = @Price, 
                                                       slug = @NewSlug,
                                                       description = @Description
                                                   WHERE slug = @OldSlug AND isdeleted = false;
                                                   """;

    private const string UpdateFlowerWithoutSlugBySlugSql = """
                                                      UPDATE flowers
                                                      SET price = @Price, description = @Description
                                                      WHERE slug = @NewSlug AND isdeleted = false;
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