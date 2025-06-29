using Flowery.WebApi.Flowers.Requests;
using Flowery.WebApi.Flowers.Responses;

namespace Flowery.WebApi.Flowers.Mappers;

public static class FlowerMapper
{
    public static FlowerResponse ToFlowerResponse(Flower entity)
    {
        return new FlowerResponse
        {
            Id = entity.Id,
            Name = entity.FlowerName.Name
        };
    }

    public static Flower ToEntity(FlowerRequest request)
    {
        return new Flower
        {
            // Name = request.Name
        };
    }
}