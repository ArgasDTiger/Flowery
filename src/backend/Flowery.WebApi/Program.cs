using Flowery.WebApi;
using Flowery.WebApi.Infrastructure.Serialization;
using Flowery.WebApi.Shared;
using Flowery.WebApi.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = new JsonContext();
});

builder.Services.AddServices(builder.Configuration);
builder.Services.AddFeatures();
builder.Services.AddSharedFeatures();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapFeatures();

app.Run();