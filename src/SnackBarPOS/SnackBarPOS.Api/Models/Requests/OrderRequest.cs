namespace SnackBarPOS.Api.Models.Requests;

public record AddOrderItemRequest(
    int ProductId,
    int Quantity
);

public record UpdateOrderItemRequest(
    int Quantity
);

public record PayOrderRequest(
    string PaymentMethod,
    decimal? AmountTendered,
    string? Notes
);
