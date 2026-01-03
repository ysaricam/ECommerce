using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Intrefaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetInStockProductsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
}