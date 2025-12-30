using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Intrefaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<List<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken);
    Task<bool> SkuExistAsync(string sku, CancellationToken cancellationToken = default);
}