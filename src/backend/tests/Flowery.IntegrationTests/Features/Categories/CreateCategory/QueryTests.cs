using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Flowery.Shared.Enums;
using Flowery.WebApi.Features.Categories.CreateCategory;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.CreateCategory;

[Collection(nameof(WritableTestsCollection))]
public sealed class QueryTests : IAsyncLifetime
{
    private readonly WritableFloweryApiFactory _factory;

    public QueryTests(WritableFloweryApiFactory factory)
    {
        _factory = factory;
    }

    public async ValueTask InitializeAsync()
    {
        await SeedTestData();
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    [Fact]
    public async Task CreateCategory_ShouldPersistCategoryWithAllNames()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var model = new DatabaseModel(
            Slug: "tropical-flowers",
            CategoryNames:
            [
                new CategoryNameRequest("Тропічні квіти", LanguageCode.UA),
                new CategoryNameRequest("Flori tropicale", LanguageCode.RO)
            ]);

        // Act
        await query.CreateCategory(model, CancellationToken.None);

        // Assert
        var category = await GetCategoryBySlug(scope, "tropical-flowers");
        category.ShouldNotBeNull();

        var names = await GetCategoryNames(scope, category.Id);
        names.Length.ShouldBe(2);
        names.ShouldContain(n => n.Name == "Тропічні квіти");
        names.ShouldContain(n => n.Name == "Flori tropicale");
    }

    [Fact]
    public async Task CreateCategory_ShouldPersistCategory_WhenOnlySingleLanguageProvided()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var model = new DatabaseModel(
            Slug: "field-flowers",
            CategoryNames:
            [
                new CategoryNameRequest("Польові квіти", LanguageCode.UA)
            ]);

        // Act
        await query.CreateCategory(model, CancellationToken.None);

        // Assert
        var category = await GetCategoryBySlug(scope, "field-flowers");
        category.ShouldNotBeNull();

        var names = await GetCategoryNames(scope, category.Id);
        names.Length.ShouldBe(1);
        names.ShouldContain(n => n.Name == "Польові квіти");
    }

    [Fact]
    public async Task CategoryExists_ShouldReturnTrue_WhenNameAlreadyExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.CategoryExists("Весільні квіти", CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task CategoryExists_ShouldReturnFalse_WhenNameDoesNotExist()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.CategoryExists("Неіснуючі квіти", CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task CategoryExists_ShouldBeCaseSensitive()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.CategoryExists("весільні квіти", CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task CreateCategory_ShouldGenerateUniqueIdPerCategory()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        var modelA = new DatabaseModel("alpine-flowers",
            [new CategoryNameRequest("Альпійські квіти", LanguageCode.UA)]);
        var modelB = new DatabaseModel("desert-flowers", [new CategoryNameRequest("Пустельні квіти", LanguageCode.UA)]);

        // Act
        await query.CreateCategory(modelA, CancellationToken.None);
        await query.CreateCategory(modelB, CancellationToken.None);

        // Assert
        var categoryA = await GetCategoryBySlug(scope, "alpine-flowers");
        var categoryB = await GetCategoryBySlug(scope, "desert-flowers");

        categoryA.ShouldNotBeNull();
        categoryB.ShouldNotBeNull();
        categoryA.Id.ShouldNotBe(categoryB.Id);
    }

    private async ValueTask SeedTestData()
    {
        var dbConnectionFactory = _factory.Services.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        await connection.ExecuteAsync(SeedCategories.SeedCategoriesSql);
    }

    private static async ValueTask<CategoryRow?> GetCategoryBySlug(IServiceScope scope, string slug)
    {
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        return await connection.QuerySingleOrDefaultAsync<CategoryRow?>(
            "SELECT Id, Slug FROM categories WHERE Slug = @slug",
            new { slug });
    }

    private static async ValueTask<CategoryNameRow[]> GetCategoryNames(IServiceScope scope, Guid categoryId)
    {
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        var results = await connection.QueryAsync<CategoryNameRow>(
            "SELECT Name, LanguageCode::text as LanguageCode FROM categoryname WHERE categoryid = @categoryId",
            new { categoryId });
        return results.ToArray();
    }

    private sealed record CategoryRow(Guid Id, string Slug);

    private sealed record CategoryNameRow(string Name, string LanguageCode);
}