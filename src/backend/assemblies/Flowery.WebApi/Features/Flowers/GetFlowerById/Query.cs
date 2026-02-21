using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly string _defaultLanguageCode;
    private readonly string _imagePrefix;

    public Query(
        IDbConnectionFactory dbConnectionFactory, 
        IOptions<TranslationConfiguration> translationSettings, 
        IOptions<ImagePathConfiguration> imagePathSettings)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _defaultLanguageCode = translationSettings.Value.SlugDefaultLanguageString;
        _imagePrefix = imagePathSettings.Value.DefaultImagePath;
    }

    public async Task<Response?> GetFlowerBySlug(string slug, LanguageCode languageCode,
        CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        object flowerParams = new
        {
            Slug = slug, LanguageCode = languageCode.ToString(),
            DefaultLanguageCode = _defaultLanguageCode
        };

        var accumulator = new Dictionary<string, FlowerAccumulator>();

        await dbConnection.QueryAsync<FlowerAccumulator, FlowerImageRow, FlowerAccumulator>(
            GetFlowerBySlugSql,
            (flower, image) =>
            {
                if (!accumulator.TryGetValue(flower.Slug, out var existing))
                {
                    existing = flower;
                    accumulator.Add(existing.Slug, existing);
                }

                if (image.IsPrimary)
                {
                    existing.PrimaryImageUrl = _imagePrefix + image.CompressedPath;
                }
                else
                {
                    existing.GalleryImageUrls.Add(_imagePrefix + image.CompressedPath);
                }

                return existing;
            },
            flowerParams,
            splitOn: nameof(FlowerImageRow.CompressedPath));

        return accumulator.Values
            .AsValueEnumerable()
            .Select(f => new Response(
                Name: f.Name,
                Slug: f.Slug,
                Price: f.Price,
                PrimaryImageUrl: f.PrimaryImageUrl,
                GalleryImageUrls: f.GalleryImageUrls.ToArray()))
            .FirstOrDefault();
    }

    private sealed record FlowerImageRow(string CompressedPath, bool IsPrimary);

    private sealed record FlowerAccumulator(string Name, string Slug, decimal Price)
    {
        public string PrimaryImageUrl { get; set; } = null!;
        public ImmutableArray<string>.Builder GalleryImageUrls { get; } = ImmutableArray.CreateBuilder<string>();
    }

    private const string GetFlowerBySlugSql =
        $"""
        SELECT 
            COALESCE(fn_requested.Name, fn_default.Name) AS {nameof(FlowerAccumulator.Name)}, 
            f.Slug AS {nameof(FlowerAccumulator.Slug)}, 
            f.Price AS {nameof(FlowerAccumulator.Price)}, 
            i.CompressedPath AS {nameof(FlowerImageRow.CompressedPath)},
            fi.IsPrimary AS {nameof(FlowerImageRow.IsPrimary)}
        FROM Flowers f
        LEFT JOIN FlowerName fn_requested ON f.Id = fn_requested.FlowerId AND fn_requested.LanguageCode = @LanguageCode::LanguageCode
        LEFT JOIN FlowerName fn_default ON f.Id = fn_default.FlowerId AND fn_default.LanguageCode = @DefaultLanguageCode::LanguageCode
        JOIN FlowerImage fi ON f.Id = fi.FlowerId
        JOIN Image i ON fi.ImageId = i.Id
        WHERE f.Slug = @Slug AND f.IsDeleted = false
        """;
}