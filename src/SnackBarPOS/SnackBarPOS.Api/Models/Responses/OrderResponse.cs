namespace SnackBarPOS.Api.Models.Responses;

public record OrderItemResponse(
    int Id,
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal
);

public record OrderResponse(
    int Id,
    string OrderNumber,
    string Status,
    decimal TotalAmount,
    string? PaymentMethod,
    decimal? AmountTendered,
    decimal? Change,
    string? Notes,
    DateTime CreatedAt,
    DateTime? PaidAt,
    DateTime? CancelledAt,
    IEnumerable<OrderItemResponse> Items
);

public record DailySummaryResponse(
    int TotalOrders,
    decimal TotalRevenue,
    int TotalItems,
    Dictionary<string, decimal> RevenueByCategory
);
