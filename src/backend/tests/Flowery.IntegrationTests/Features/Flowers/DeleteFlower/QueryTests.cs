using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Microsoft.Extensions.DependencyInjection;
using Flowery.WebApi.Features.Flowers.DeleteFlower;

namespace Flowery.IntegrationTests.Features.Flowers.DeleteFlower;

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
    public async Task DeleteFlowerBySlug_ShouldReturnSuccess_WhenFlowerExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        const string slug = "rose";
        var guid = Guid.Parse("7c77c1b8-0889-4685-9d84-6eb4500f8926");

        // Act
        var result = await query.DeleteFlowerBySlug(slug, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
        var flower = await GetFlowerById(scope, guid);
        flower!.ShouldBeNull();
    }

    private async ValueTask SeedTestData()
    {
        var dbConnectionFactory = _factory.Services.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        await connection.ExecuteAsync(SeedFlowers.SeedFlowersSql);
    }

    private static async ValueTask<dynamic?> GetFlowerById(IServiceScope scope, Guid id)
    {
        const string sql = "SELECT * FROM flowers WHERE id = @id AND isdeleted = true";
        var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        return await connection.QuerySingleOrDefaultAsync(sql, new { id });
    }
}