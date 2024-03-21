using Saga.Orchestrator.API.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.RegisterServices();
builder.Services.AddMassTransitConfiguration();

var app = builder.Build();
app.UseApiConfiguration();
app.Run();
