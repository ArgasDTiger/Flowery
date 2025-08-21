namespace Flowery.WebApi.Shared.Features;

public interface IFeature
{
    static abstract void Register(IServiceCollection services);
    static abstract void MapEndpoint(IEndpointRouteBuilder endpoints);
}