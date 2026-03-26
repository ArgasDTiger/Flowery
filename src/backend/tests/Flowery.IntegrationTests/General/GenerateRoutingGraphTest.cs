using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.General;

// https://dreampuf.github.io/GraphvizOnline/?engine=dot
public sealed class GenerateRoutingGraphTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public GenerateRoutingGraphTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    [Fact(Skip = "Only for manual testing")]
    public void GenerateGraph()
    {
        var graphWriter = _factory.Services.GetRequiredService<DfaGraphWriter>();
        var endpointData = _factory.Services.GetRequiredService<EndpointDataSource>();

        using (var sw = new StringWriter())
        {
            graphWriter.Write(endpointData, sw);
            var graph = sw.ToString();

            _output.WriteLine(graph);
        }
    }
}