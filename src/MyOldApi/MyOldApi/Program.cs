using Microsoft.EntityFrameworkCore;
using MyOldApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MlbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MlbConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("MyOldApi");

var app = builder.Build();

// Ensure the SQLite database and schema exist on startup
using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<MlbContext>();
  db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
