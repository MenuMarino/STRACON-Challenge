using SupplierPortalAPI.Repositories;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByRequestIdAsync(int requestId)
        {
            return await _productRepository.GetProductsByRequestIdAsync(requestId);
        }
    }
}
