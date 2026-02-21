using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Flowery.WebApi.Features.Categories.DeleteCategory;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.DeleteCategory;

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
    public async Task DeleteCategoryById_ShouldReturnSuccess_AndRemoveAllRelatedData_WhenCategoryExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var categoryId = Guid.Parse("a1a1a1a1-1111-1111-1111-111111111111");

        // Act
        await query.DeleteCategoryById(categoryId, CancellationToken.None);

        // Assert
        await CategoryShouldNotExist(scope, categoryId);
        await CategoryNamesShouldNotExist(scope, categoryId);
    }

    [Fact]
    public async Task GetCategoryIdBySlug_ShouldReturnId_WhenCategoryExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetCategoryIdBySlug("wedding-flowers", CancellationToken.None);

        // Assert
        result.ShouldBe(Guid.Parse("a1a1a1a1-1111-1111-1111-111111111111"));
    }

    [Fact]
    public async Task GetCategoryIdBySlug_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetCategoryIdBySlug("not-a-category", CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }

    private async ValueTask SeedTestData()
    {
        var dbConnectionFactory = _factory.Services.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        await connection.ExecuteAsync(SeedCategories.SeedCategoriesSql);
    }

    private static async ValueTask CategoryShouldNotExist(IServiceScope scope, Guid categoryId)
    {
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        bool categoryExists = await connection.QuerySingleOrDefaultAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM categories WHERE Id = @categoryId)",
            new { categoryId });
        categoryExists.ShouldBeFalse();
    }

    private static async ValueTask CategoryNamesShouldNotExist(IServiceScope scope, Guid categoryId)
    {
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        bool namesExist = await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM categoryname WHERE categoryid = @categoryId)",
            new { categoryId });
        namesExist.ShouldBeFalse();
    }
}