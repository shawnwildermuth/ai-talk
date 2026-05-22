using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SnackBarPOS.Api.Data;
using SnackBarPOS.Api.Endpoints;
using SnackBarPOS.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddOpenApi();

// CORS for Vue frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "https://localhost:5173",
                "https://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseCors("VueFrontend");

// OpenAPI / Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opts =>
    {
        opts.Title = "SnackBar POS API";
        opts.Theme = ScalarTheme.BluePlanet;
    });
}

// Map endpoints
app.MapCategoryEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();

app.Run();

public partial class Program { }
