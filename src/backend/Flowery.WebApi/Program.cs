using System.Text.Json.Serialization;
using Flowery.WebApi;
using Flowery.WebApi.Infrastructure.Health;
using Flowery.WebApi.Shared;
using Flowery.WebApi.Shared.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddServices(builder.Configuration);
builder.Services.AddFeatures();
builder.Services.AddSharedFeatures();
builder.Services.AddConfigurations(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

// TODO: Add authentication
app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapFeatures();

app.Run();