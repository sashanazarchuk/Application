using EventSystem.API.Extensions;
using EventSystem.API.Middlewares;
using EventSystem.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

// Add NLog logging services
builder.AddLoggingServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwaggerDev(app.Environment);

// Seed the database
await app.SeedAllAsync();

// Use custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunWithNLog();