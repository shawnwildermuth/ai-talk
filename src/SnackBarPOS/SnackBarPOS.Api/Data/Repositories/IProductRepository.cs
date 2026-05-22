using SnackBarPOS.Api.Entities;

namespace SnackBarPOS.Api.Data.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken ct = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Product> CreateAsync(Product product, CancellationToken ct = default);
    Task<Product> UpdateAsync(Product product, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
