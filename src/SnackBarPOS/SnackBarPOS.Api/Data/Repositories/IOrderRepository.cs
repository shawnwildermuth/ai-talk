using SnackBarPOS.Api.Entities;

namespace SnackBarPOS.Api.Data.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync(DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Order?> GetOpenOrderAsync(CancellationToken ct = default);
    Task<Order> CreateAsync(Order order, CancellationToken ct = default);
    Task<Order> UpdateAsync(Order order, CancellationToken ct = default);
    Task<bool> AddItemAsync(int orderId, OrderItem item, CancellationToken ct = default);
    Task<bool> RemoveItemAsync(int orderId, int itemId, CancellationToken ct = default);
    Task<bool> UpdateItemQuantityAsync(int orderId, int itemId, int quantity, CancellationToken ct = default);
    Task<string> GenerateOrderNumberAsync(CancellationToken ct = default);
    Task<DailySummary> GetDailySummaryAsync(DateTime date, CancellationToken ct = default);
}

public record DailySummary(int TotalOrders, decimal TotalRevenue, int TotalItems, Dictionary<string, decimal> RevenueByCategory);
