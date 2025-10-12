using EventSystem.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiServices();

// Add NLog logging services
builder.AddLoggingServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwaggerDev(app.Environment);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunWithNLog();