namespace SnackBarPOS.Api.Entities;

public enum OrderStatus
{
    Open = 0,
    Paid = 1,
    Cancelled = 2
}

public enum PaymentMethod
{
    Cash = 0,
    Pin = 1,
    Contactless = 2
}

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Open;
    public decimal TotalAmount { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public decimal? AmountTendered { get; set; }
    public decimal? Change { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Parse("2026-05-05").ToUniversalTime();
    public DateTime? PaidAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = [];
}
