var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Flowery_WebApi>("web-api");

builder.Build().Run();