using System.Text.Json.Serialization;
using Flowery.WebApi.Features.Flowers.GetFlowers;
using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.Infrastructure.Serialization;

[JsonSerializable(typeof(Request))]
[JsonSerializable(typeof(Response))]
[JsonSerializable(typeof(IEnumerable<Response>))]
[JsonSerializable(typeof(SortField))]
[JsonSerializable(typeof(SortDirection))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    Converters = [typeof(SortDirectionConverter), typeof(SortFieldConverter)]
)]
public partial class JsonContext : JsonSerializerContext;