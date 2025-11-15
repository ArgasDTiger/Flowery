using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.WebApi.Features.Flowers.GetFlowers;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Flowers.GetFlowers;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class QueryTests
{
    private readonly ReadonlyFloweryApiFactory _factory;

    public QueryTests(ReadonlyFloweryApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetFlowers_ShouldReturnFlowers()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        var request = new Request
        {
            PageNumber = 1,
            PageSize = 3,
            SortDirectionString = "asc",
            SortFieldString = "Name"
        };

        // Act
        var result = await query.GetFlowers(request, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(12);
    }
}