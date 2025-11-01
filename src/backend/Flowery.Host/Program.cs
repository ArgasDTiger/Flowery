var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Flowery_WebApi>("web-api");

if (args.Contains("--migrations-run"))
{
    builder.AddProject<Projects.Flowery_Migrations>("migrations");
}

builder.AddProject<Projects.Flowery_WebApi_UnitTests>("web-api-tests");

builder.Build().Run();