using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Flowery.Shared.Enums;
using Flowery.WebApi.Features.Categories.UpdateCategory;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.UpdateCategory;

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
    public async Task UpdateCategory_ShouldUpdateNames_WhenNameUnchanged()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var model = new DatabaseModel(
            Id: Guid.Parse("b2b2b2b2-2222-2222-2222-222222222222"),
            OldSlug: "garden-flowers",
            NewSlug: null,
            CategoryNames:
            [
                new CategoryNameRequest("Садові квіти оновлені", LanguageCode.UA)
            ],
            NameChanged: false);

        // Act
        var result = await query.UpdateCategory(model, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
        var name = await GetCategoryName(scope, model.Id, LanguageCode.UA);
        name.ShouldBe("Садові квіти оновлені");
    }

    [Fact]
    public async Task UpdateCategory_ShouldUpdateSlug_WhenNameChanged()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var model = new DatabaseModel(
            Id: Guid.Parse("c3c3c3c3-3333-3333-3333-333333333333"),
            OldSlug: "exotic-flowers",
            NewSlug: "rare-flowers",
            CategoryNames:
            [
                new CategoryNameRequest("Рідкісні квіти", LanguageCode.UA)
            ],
            NameChanged: true);

        // Act
        var result = await query.UpdateCategory(model, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
        var slug = await GetCategorySlug(scope, model.Id);
        slug.ShouldBe("rare-flowers");
    }

    [Fact]
    public async Task UpdateCategory_ShouldRemoveNames_WhenLanguageNoLongerPresent()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var model = new DatabaseModel(
            Id: Guid.Parse("d4d4d4d4-4444-4444-4444-444444444444"),
            OldSlug: "spring-flowers",
            NewSlug: null,
            CategoryNames:
            [
                new CategoryNameRequest("Весняні квіти", LanguageCode.UA)
            ],
            NameChanged: false);

        // Act
        await query.UpdateCategory(model, CancellationToken.None);

        // Assert
        var roName = await GetCategoryName(scope, model.Id, LanguageCode.RO);
        roName.ShouldBeNull();
    }

    private async ValueTask SeedTestData()
    {
        var dbConnectionFactory = _factory.Services.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        await connection.ExecuteAsync(SeedCategories.SeedCategoriesSql);
    }

    private static async ValueTask<string?> GetCategoryName(IServiceScope scope, Guid categoryId, LanguageCode languageCode)
    {
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        return await connection.QuerySingleOrDefaultAsync<string?>(
            "SELECT name FROM categoryname WHERE categoryid = @categoryId AND languagecode = @languageCode::LanguageCode",
            new { categoryId, languageCode = languageCode.ToString() });
    }

    private static async ValueTask<string?> GetCategorySlug(IServiceScope scope, Guid categoryId)
    {
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        return await connection.QuerySingleOrDefaultAsync<string?>(
            "SELECT slug FROM categories WHERE id = @categoryId",
            new { categoryId });
    }
}