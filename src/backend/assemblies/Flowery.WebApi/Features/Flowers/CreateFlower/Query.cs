using Dapper;
using Flowery.Infrastructure.Data;
using Npgsql;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task CreateFlower(DatabaseModel model, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection =
            (NpgsqlConnection)await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            await connection.ExecuteAsync(InsertFlowerSql, new
            {
                FlowerId = model.Id,
                model.Price,
                model.Slug,
                model.Description,
                PrimaryImageId = model.PrimaryImage.Id,
                PrimaryImagePathToSource = model.PrimaryImage.PathToSource,
                PrimaryImageCompressedPath = model.PrimaryImage.CompressedPath,
                PrimaryImageThumbnailPath = model.PrimaryImage.ThumbnailPath,
                FlowerNames = model.FlowerNames.Select(fn => fn.Name).ToArray(),
                FlowerNameLanguageCodes = model.FlowerNames.Select(fn => fn.LanguageCode.ToString()).ToArray(),
                GalleryImageIds = model.GalleryImages.Select(i => i.Id).ToArray(),
                GalleryImagePathsToSource = model.GalleryImages.Select(i => i.PathToSource).ToArray(),
                GalleryImageCompressedPaths = model.GalleryImages.Select(i => i.CompressedPath).ToArray(),
                GalleryImageThumbnailPaths = model.GalleryImages.Select(i => i.ThumbnailPath).ToArray(),
            }, transaction);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private const string InsertFlowerSql =
        """
        WITH insert_primary_image AS (
            INSERT INTO image (Id, PathToSource, CompressedPath, ThumbnailPath)
            VALUES (@PrimaryImageId, @PrimaryImagePathToSource, @PrimaryImageCompressedPath, @PrimaryImageThumbnailPath)
        ),
        insert_flower AS (
            INSERT INTO flowers (Id, Price, Slug, Description)
            VALUES (@FlowerId, @Price, @Slug, @Description)
        ),
        insert_flower_names AS (
            INSERT INTO flowername (FlowerId, LanguageCode, Name)
            SELECT @FlowerId, UNNEST(@FlowerNameLanguageCodes::LanguageCode[]), UNNEST(@FlowerNames)
        ),
        insert_primary_flower_image AS (
            INSERT INTO flowerimage (FlowerId, ImageId, IsPrimary)
            VALUES (@FlowerId, @PrimaryImageId, TRUE)
        ),
        insert_gallery_images AS (
            INSERT INTO image (Id, PathToSource, CompressedPath, ThumbnailPath)
            SELECT UNNEST(@GalleryImageIds::uuid[]),
                   UNNEST(@GalleryImagePathsToSource::varchar(255)[]),
                   UNNEST(@GalleryImageCompressedPaths::varchar(255)[]),
                   UNNEST(@GalleryImageThumbnailPaths::varchar(255)[])
            WHERE array_length(@GalleryImageIds::uuid[], 1) IS NOT NULL
        )
        INSERT INTO flowerimage (FlowerId, ImageId, IsPrimary)
        SELECT @FlowerId, UNNEST(@GalleryImageIds::uuid[]), FALSE
        WHERE array_length(@GalleryImageIds::uuid[], 1) IS NOT NULL;
        """;
}