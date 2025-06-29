using FluentValidation;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();

app.Run();