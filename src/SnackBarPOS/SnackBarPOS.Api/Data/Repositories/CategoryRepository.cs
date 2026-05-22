using Microsoft.EntityFrameworkCore;
using SnackBarPOS.Api.Entities;

namespace SnackBarPOS.Api.Data.Repositories;

public class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default) =>
        await db.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync(ct);

    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Category> CreateAsync(Category category, CancellationToken ct = default)
    {
        db.Categories.Add(category);
        await db.SaveChangesAsync(ct);
        return category;
    }

    public async Task<Category> UpdateAsync(Category category, CancellationToken ct = default)
    {
        db.Categories.Update(category);
        await db.SaveChangesAsync(ct);
        return category;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var category = await GetByIdAsync(id, ct);
        if (category is null) return false;
        category.IsActive = false;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        db.Categories.AnyAsync(c => c.Id == id, ct);
}
