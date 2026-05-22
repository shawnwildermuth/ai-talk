using FluentValidation;
using Mapster;
using SnackBarPOS.Api.Data.Repositories;
using SnackBarPOS.Api.Entities;
using SnackBarPOS.Api.Models.Requests;
using SnackBarPOS.Api.Models.Responses;

namespace SnackBarPOS.Api.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories").WithTags("Categorieën");

        group.MapGet("/", async (ICategoryRepository repo, CancellationToken ct) =>
        {
            var categories = await repo.GetAllAsync(ct);
            return Results.Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        })
        .WithName("GetCategories")
        .WithSummary("Haal alle categorieën op");

        group.MapGet("/{id:int}", async (int id, ICategoryRepository repo, CancellationToken ct) =>
        {
            var category = await repo.GetByIdAsync(id, ct);
            return category is null
                ? Results.NotFound(new { message = $"Categorie met id {id} niet gevonden" })
                : Results.Ok(category.Adapt<CategoryResponse>());
        })
        .WithName("GetCategoryById")
        .WithSummary("Haal een categorie op via id");

        group.MapPost("/", async (
            CreateCategoryRequest request,
            IValidator<CreateCategoryRequest> validator,
            ICategoryRepository repo,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var category = request.Adapt<Category>();
            var created = await repo.CreateAsync(category, ct);
            return Results.Created($"/api/categories/{created.Id}", created.Adapt<CategoryResponse>());
        })
        .WithName("CreateCategory")
        .WithSummary("Maak een nieuwe categorie aan");

        group.MapPut("/{id:int}", async (
            int id,
            UpdateCategoryRequest request,
            IValidator<UpdateCategoryRequest> validator,
            ICategoryRepository repo,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var category = await repo.GetByIdAsync(id, ct);
            if (category is null)
                return Results.NotFound(new { message = $"Categorie met id {id} niet gevonden" });

            request.Adapt(category);
            var updated = await repo.UpdateAsync(category, ct);
            return Results.Ok(updated.Adapt<CategoryResponse>());
        })
        .WithName("UpdateCategory")
        .WithSummary("Werk een categorie bij");

        group.MapDelete("/{id:int}", async (int id, ICategoryRepository repo, CancellationToken ct) =>
        {
            var deleted = await repo.DeleteAsync(id, ct);
            return deleted
                ? Results.NoContent()
                : Results.NotFound(new { message = $"Categorie met id {id} niet gevonden" });
        })
        .WithName("DeleteCategory")
        .WithSummary("Verwijder een categorie (soft delete)");

        return app;
    }
}
