using Microsoft.EntityFrameworkCore;
using SupplierPortalAPI.Data;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SupplierPortalContext _context;

        public ProductRepository(SupplierPortalContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<IEnumerable<Product>> GetProductsByRequestIdAsync(int requestId)
        {
            return await _context.Products
                .Where(p => p.RequestId == requestId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
