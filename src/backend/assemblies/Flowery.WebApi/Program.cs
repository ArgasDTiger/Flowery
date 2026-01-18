using System.Text.Json.Serialization;
using Flowery.WebApi;
using Flowery.WebApi.Shared;
using Flowery.WebApi.Shared.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

const string solutionName = "Flowery";
var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddServices(builder.Configuration);
builder.Services.AddFeatures();
builder.Services.AddSharedFeatures();
builder.Services.AddConfigurations(builder.Configuration);

builder.Services.AddOpenApi(opt =>
{
    opt.CreateSchemaReferenceId = ctx =>
    {
        var type = ctx.Type;
        string[] targetNames = ["Request", "Response"];
        if (type.Namespace is not null && type.Namespace.StartsWith(solutionName) && targetNames.Contains(type.Name))
        {
            string folderName = type.Namespace.Split('.').Last();
            return $"{folderName}{type.Name}";
        }
        return null; 
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

// TODO: Add authentication
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapFeatures();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Scalar is available at: http://localhost:5200/scalar");
}

app.Run();