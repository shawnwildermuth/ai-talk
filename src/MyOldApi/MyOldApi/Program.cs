using Microsoft.EntityFrameworkCore;
using MyOldApi.Controllers;
using MyOldApi.Data;
using MyOldApi.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<WorldCupContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WorldCupConnection")));
builder.Services.AddScoped<IWorldCupRepository, WorldCupRepository>();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("MyOldApi");

var app = builder.Build();

// Ensure the SQLite database and schema exist on startup
using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<WorldCupContext>();
  db.Database.EnsureDeleted();
  db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapTeamsEndpoints();
app.MapPlayersEndpoints();

app.Run();
