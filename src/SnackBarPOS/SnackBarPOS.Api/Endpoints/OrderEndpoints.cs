using FluentValidation;
using Mapster;
using SnackBarPOS.Api.Data.Repositories;
using SnackBarPOS.Api.Entities;
using SnackBarPOS.Api.Models.Requests;
using SnackBarPOS.Api.Models.Responses;

namespace SnackBarPOS.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Bestellingen");

        // GET all orders (with optional date filter)
        group.MapGet("/", async (
            IOrderRepository repo,
            DateTime? from,
            DateTime? to,
            CancellationToken ct) =>
        {
            var orders = await repo.GetAllAsync(from, to, ct);
            return Results.Ok(orders.Adapt<IEnumerable<OrderResponse>>());
        })
        .WithName("GetOrders")
        .WithSummary("Haal alle bestellingen op");

        // GET daily summary
        group.MapGet("/summary/today", async (IOrderRepository repo, CancellationToken ct) =>
        {
            var summary = await repo.GetDailySummaryAsync(DateTime.UtcNow, ct);
            return Results.Ok(new DailySummaryResponse(
                summary.TotalOrders,
                summary.TotalRevenue,
                summary.TotalItems,
                summary.RevenueByCategory));
        })
        .WithName("GetTodaySummary")
        .WithSummary("Haal de dagelijkse samenvatting op");

        // GET summary for a date
        group.MapGet("/summary/{date}", async (
            DateTime date,
            IOrderRepository repo,
            CancellationToken ct) =>
        {
            var summary = await repo.GetDailySummaryAsync(date, ct);
            return Results.Ok(new DailySummaryResponse(
                summary.TotalOrders,
                summary.TotalRevenue,
                summary.TotalItems,
                summary.RevenueByCategory));
        })
        .WithName("GetSummaryByDate")
        .WithSummary("Haal de samenvatting op voor een specifieke datum");

        // GET current open order
        group.MapGet("/current", async (IOrderRepository repo, CancellationToken ct) =>
        {
            var order = await repo.GetOpenOrderAsync(ct);
            return order is null
                ? Results.NotFound(new { message = "Geen open bestelling" })
                : Results.Ok(order.Adapt<OrderResponse>());
        })
        .WithName("GetCurrentOrder")
        .WithSummary("Haal de huidige open bestelling op");

        // GET order by id
        group.MapGet("/{id:int}", async (int id, IOrderRepository repo, CancellationToken ct) =>
        {
            var order = await repo.GetByIdAsync(id, ct);
            return order is null
                ? Results.NotFound(new { message = $"Bestelling met id {id} niet gevonden" })
                : Results.Ok(order.Adapt<OrderResponse>());
        })
        .WithName("GetOrderById")
        .WithSummary("Haal een bestelling op via id");

        // POST create new order
        group.MapPost("/", async (IOrderRepository repo, CancellationToken ct) =>
        {
            // Check if there's already an open order
            var existing = await repo.GetOpenOrderAsync(ct);
            if (existing is not null)
                return Results.Conflict(new { message = "Er is al een open bestelling", orderId = existing.Id });

            var orderNumber = await repo.GenerateOrderNumberAsync(ct);
            var order = new Order
            {
                OrderNumber = orderNumber,
                Status = OrderStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            var created = await repo.CreateAsync(order, ct);
            return Results.Created($"/api/orders/{created.Id}", created.Adapt<OrderResponse>());
        })
        .WithName("CreateOrder")
        .WithSummary("Maak een nieuwe bestelling aan");

        // POST add item to order
        group.MapPost("/{id:int}/items", async (
            int id,
            AddOrderItemRequest request,
            IValidator<AddOrderItemRequest> validator,
            IOrderRepository orderRepo,
            IProductRepository productRepo,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var order = await orderRepo.GetByIdAsync(id, ct);
            if (order is null)
                return Results.NotFound(new { message = $"Bestelling met id {id} niet gevonden" });

            if (order.Status != OrderStatus.Open)
                return Results.BadRequest(new { message = "Kan alleen items toevoegen aan een open bestelling" });

            var product = await productRepo.GetByIdAsync(request.ProductId, ct);
            if (product is null)
                return Results.NotFound(new { message = $"Product met id {request.ProductId} niet gevonden" });

            if (!product.IsAvailable)
                return Results.BadRequest(new { message = $"Product '{product.Name}' is momenteel niet beschikbaar" });

            var item = new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = request.Quantity
            };

            var success = await orderRepo.AddItemAsync(id, item, ct);
            if (!success)
                return Results.BadRequest(new { message = "Kan item niet toevoegen aan bestelling" });

            var updatedOrder = await orderRepo.GetByIdAsync(id, ct);
            return Results.Ok(updatedOrder!.Adapt<OrderResponse>());
        })
        .WithName("AddOrderItem")
        .WithSummary("Voeg een item toe aan een bestelling");

        // PATCH update item quantity
        group.MapPatch("/{id:int}/items/{itemId:int}", async (
            int id,
            int itemId,
            UpdateOrderItemRequest request,
            IValidator<UpdateOrderItemRequest> validator,
            IOrderRepository repo,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var success = await repo.UpdateItemQuantityAsync(id, itemId, request.Quantity, ct);
            if (!success)
                return Results.NotFound(new { message = "Bestelling of item niet gevonden" });

            var updatedOrder = await repo.GetByIdAsync(id, ct);
            return Results.Ok(updatedOrder!.Adapt<OrderResponse>());
        })
        .WithName("UpdateOrderItem")
        .WithSummary("Werk de hoeveelheid van een item bij");

        // DELETE remove item from order
        group.MapDelete("/{id:int}/items/{itemId:int}", async (
            int id,
            int itemId,
            IOrderRepository repo,
            CancellationToken ct) =>
        {
            var success = await repo.RemoveItemAsync(id, itemId, ct);
            if (!success)
                return Results.NotFound(new { message = "Bestelling of item niet gevonden" });

            var updatedOrder = await repo.GetByIdAsync(id, ct);
            return Results.Ok(updatedOrder!.Adapt<OrderResponse>());
        })
        .WithName("RemoveOrderItem")
        .WithSummary("Verwijder een item uit een bestelling");

        // POST pay order
        group.MapPost("/{id:int}/pay", async (
            int id,
            PayOrderRequest request,
            IValidator<PayOrderRequest> validator,
            IOrderRepository repo,
            CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var order = await repo.GetByIdAsync(id, ct);
            if (order is null)
                return Results.NotFound(new { message = $"Bestelling met id {id} niet gevonden" });

            if (order.Status != OrderStatus.Open)
                return Results.BadRequest(new { message = "Deze bestelling is al afgesloten" });

            if (!order.Items.Any())
                return Results.BadRequest(new { message = "Bestelling heeft geen items" });

            if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var paymentMethod))
                return Results.BadRequest(new { message = "Ongeldige betaalwijze" });

            if (paymentMethod == PaymentMethod.Cash && request.AmountTendered.HasValue)
            {
                if (request.AmountTendered < order.TotalAmount)
                    return Results.BadRequest(new { message = "Betaald bedrag is lager dan het totaalbedrag" });
                order.AmountTendered = request.AmountTendered;
                order.Change = request.AmountTendered - order.TotalAmount;
            }

            order.Status = OrderStatus.Paid;
            order.PaymentMethod = paymentMethod;
            order.PaidAt = DateTime.UtcNow;
            order.Notes = request.Notes;

            var updated = await repo.UpdateAsync(order, ct);
            return Results.Ok(updated.Adapt<OrderResponse>());
        })
        .WithName("PayOrder")
        .WithSummary("Betaal een bestelling");

        // POST cancel order
        group.MapPost("/{id:int}/cancel", async (
            int id,
            IOrderRepository repo,
            CancellationToken ct) =>
        {
            var order = await repo.GetByIdAsync(id, ct);
            if (order is null)
                return Results.NotFound(new { message = $"Bestelling met id {id} niet gevonden" });

            if (order.Status != OrderStatus.Open)
                return Results.BadRequest(new { message = "Alleen open bestellingen kunnen worden geannuleerd" });

            order.Status = OrderStatus.Cancelled;
            order.CancelledAt = DateTime.UtcNow;

            var updated = await repo.UpdateAsync(order, ct);
            return Results.Ok(updated.Adapt<OrderResponse>());
        })
        .WithName("CancelOrder")
        .WithSummary("Annuleer een bestelling");

        return app;
    }
}
