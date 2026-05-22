using Microsoft.EntityFrameworkCore;
using SnackBarPOS.Api.Entities;

namespace SnackBarPOS.Api.Data.Repositories;

public class OrderRepository(AppDbContext db) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllAsync(DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var query = db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Category)
            .AsQueryable();

        if (from.HasValue) query = query.Where(o => o.CreatedAt >= from.Value);
        if (to.HasValue) query = query.Where(o => o.CreatedAt <= to.Value);

        return await query.OrderByDescending(o => o.CreatedAt).ToListAsync(ct);
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<Order?> GetOpenOrderAsync(CancellationToken ct = default) =>
        await db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(o => o.Status == OrderStatus.Open, ct);

    public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
    {
        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);
        return order;
    }

    public async Task<Order> UpdateAsync(Order order, CancellationToken ct = default)
    {
        db.Orders.Update(order);
        await db.SaveChangesAsync(ct);
        return await GetByIdAsync(order.Id, ct) ?? order;
    }

    public async Task<bool> AddItemAsync(int orderId, OrderItem item, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null || order.Status != OrderStatus.Open) return false;

        // If product already in order, increment quantity
        var existing = order.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existing is not null)
        {
            existing.Quantity += item.Quantity;
        }
        else
        {
            item.OrderId = orderId;
            db.OrderItems.Add(item);
        }

        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity)
            + (existing is null ? item.UnitPrice * item.Quantity : 0);

        // Recalculate total from DB
        await db.SaveChangesAsync(ct);

        // Recalculate total properly
        var updatedOrder = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
        if (updatedOrder is not null)
        {
            updatedOrder.TotalAmount = updatedOrder.Items.Sum(i => i.UnitPrice * i.Quantity);
            await db.SaveChangesAsync(ct);
        }

        return true;
    }

    public async Task<bool> RemoveItemAsync(int orderId, int itemId, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null || order.Status != OrderStatus.Open) return false;

        var item = order.Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null) return false;

        db.OrderItems.Remove(item);
        await db.SaveChangesAsync(ct);

        order.TotalAmount = order.Items
            .Where(i => i.Id != itemId)
            .Sum(i => i.UnitPrice * i.Quantity);
        await db.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> UpdateItemQuantityAsync(int orderId, int itemId, int quantity, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null || order.Status != OrderStatus.Open) return false;

        var item = order.Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null) return false;

        if (quantity <= 0)
        {
            db.OrderItems.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }

        await db.SaveChangesAsync(ct);

        var updatedOrder = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
        if (updatedOrder is not null)
        {
            updatedOrder.TotalAmount = updatedOrder.Items.Sum(i => i.UnitPrice * i.Quantity);
            await db.SaveChangesAsync(ct);
        }

        return true;
    }

    public async Task<string> GenerateOrderNumberAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        var prefix = today.ToString("yyyyMMdd");
        var count = await db.Orders
            .CountAsync(o => o.CreatedAt.Date == today, ct);
        return $"{prefix}-{(count + 1):D4}";
    }

    public async Task<DailySummary> GetDailySummaryAsync(DateTime date, CancellationToken ct = default)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        var orders = await db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Category)
            .Where(o => o.CreatedAt >= startOfDay && o.CreatedAt < endOfDay && o.Status == OrderStatus.Paid)
            .ToListAsync(ct);

        var totalOrders = orders.Count;
        var totalRevenue = orders.Sum(o => o.TotalAmount);
        var totalItems = orders.SelectMany(o => o.Items).Sum(i => i.Quantity);

        var revenueByCategory = orders
            .SelectMany(o => o.Items)
            .GroupBy(i => i.Product.Category.Name)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.UnitPrice * i.Quantity));

        return new DailySummary(totalOrders, totalRevenue, totalItems, revenueByCategory);
    }
}
