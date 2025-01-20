using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsByRequestIdAsync(int requestId);
        Task SaveChangesAsync();
    }
}
