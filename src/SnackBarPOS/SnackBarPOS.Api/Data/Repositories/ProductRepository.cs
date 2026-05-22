using Microsoft.EntityFrameworkCore;
using SnackBarPOS.Api.Entities;

namespace SnackBarPOS.Api.Data.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default) =>
        await db.Products
            .Include(p => p.Category)
            .Where(p => includeInactive || p.IsActive)
            .OrderBy(p => p.Category.SortOrder)
            .ThenBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken ct = default) =>
        await db.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Product> CreateAsync(Product product, CancellationToken ct = default)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync(ct);
        return await GetByIdAsync(product.Id, ct) ?? product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken ct = default)
    {
        product.UpdatedAt = DateTime.UtcNow;
        db.Products.Update(product);
        await db.SaveChangesAsync(ct);
        return await GetByIdAsync(product.Id, ct) ?? product;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var product = await GetByIdAsync(id, ct);
        if (product is null) return false;
        product.IsActive = false;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        db.Products.AnyAsync(p => p.Id == id, ct);
}
