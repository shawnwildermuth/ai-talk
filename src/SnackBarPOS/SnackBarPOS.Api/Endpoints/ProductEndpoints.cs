using FluentValidation;
using Mapster;
using SnackBarPOS.Api.Data.Repositories;
using SnackBarPOS.Api.Entities;
using SnackBarPOS.Api.Models.Requests;
using SnackBarPOS.Api.Models.Responses;

namespace SnackBarPOS.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Producten");

        group.MapGet("/", async (
            ICategoryRepository categoryRepo,
            IProductRepository productRepo,
            bool? includeInactive,
            int? categoryId,
            CancellationToken ct) =>
        {
            IEnumerable<Product> products;

            if (categoryId.HasValue)
                products = await productRepo.GetByCategoryAsync(categoryId.Value, ct);
            else
                products = await productRepo.GetAllAsync(includeInactive ?? false, ct);

            return Results.Ok(products.Adapt<IEnumerable<ProductResponse>>());
        })
        .WithName("GetProducts")
        .WithSummary("Haal alle producten op");

        group.MapGet("/{id:int}", async (int id, IProductRepository repo, CancellationToken ct) =>
        {
            var product = await repo.GetByIdAsync(id, ct);
            return product is null
                ? Results.NotFound(new { message = $"Product met id {id} niet gevonden" })
                : Results.Ok(product.Adapt<ProductResponse>());
        })
        .WithName("GetProductById")
        .WithSummary("Haal een product op via id");

        group.MapPost("/", async (
            CreateProductRequest request,
            IValidator<CreateProductRequest> validator,
            IProductRepository productRepo,
            ICategoryRepository categoryRepo,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            if (!await categoryRepo.ExistsAsync(request.CategoryId, ct))
                return Results.BadRequest(new { message = $"Categorie met id {request.CategoryId} niet gevonden" });

            var product = request.Adapt<Product>();
            var created = await productRepo.CreateAsync(product, ct);
            return Results.Created($"/api/products/{created.Id}", created.Adapt<ProductResponse>());
        })
        .WithName("CreateProduct")
        .WithSummary("Maak een nieuw product aan");

        group.MapPut("/{id:int}", async (
            int id,
            UpdateProductRequest request,
            IValidator<UpdateProductRequest> validator,
            IProductRepository productRepo,
            ICategoryRepository categoryRepo,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var product = await productRepo.GetByIdAsync(id, ct);
            if (product is null)
                return Results.NotFound(new { message = $"Product met id {id} niet gevonden" });

            if (!await categoryRepo.ExistsAsync(request.CategoryId, ct))
                return Results.BadRequest(new { message = $"Categorie met id {request.CategoryId} niet gevonden" });

            request.Adapt(product);
            var updated = await productRepo.UpdateAsync(product, ct);
            return Results.Ok(updated.Adapt<ProductResponse>());
        })
        .WithName("UpdateProduct")
        .WithSummary("Werk een product bij");

        group.MapDelete("/{id:int}", async (int id, IProductRepository repo, CancellationToken ct) =>
        {
            var deleted = await repo.DeleteAsync(id, ct);
            return deleted
                ? Results.NoContent()
                : Results.NotFound(new { message = $"Product met id {id} niet gevonden" });
        })
        .WithName("DeleteProduct")
        .WithSummary("Verwijder een product (soft delete)");

        return app;
    }
}
