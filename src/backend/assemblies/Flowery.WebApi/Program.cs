using System.Text.Json.Serialization;
using Flowery.WebApi;
using Flowery.WebApi.Shared;
using Flowery.WebApi.Shared.Extensions;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
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

// TODO: investigate need for this in Web APi
builder.Services.AddAntiforgery();

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

// TODO: add more restrictions
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", b =>
    {
        b.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddProblemDetails(opt =>
{
    opt.CustomizeProblemDetails = (ctx) =>
    {
        ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("CorsPolicy");
// app.UseHttpsRedirection();

app.UseAntiforgery();
app.UseHangfireDashboard();
app.UseStatusCodePages();
app.UseStaticFiles();

// TODO: Add authentication
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapFeatures();
app.MapHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var server = app.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;

        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        if (addresses is not null)
        {
            foreach (var address in addresses)
            {
                logger.LogInformation("Scalar is available at: {Address}/scalar", address);
                logger.LogInformation("Hangfire Dashboard is available at: {Address}/hangfire", address);
            }
        }
    });
}

app.Run();