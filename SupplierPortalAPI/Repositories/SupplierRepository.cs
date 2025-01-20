using Microsoft.EntityFrameworkCore;
using SupplierPortalAPI.Data;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SupplierPortalContext _context;

        public SupplierRepository(SupplierPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int supplierId)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.SupplierId == supplierId);
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id) ?? throw new KeyNotFoundException($"Proveedor con ID {id} no existe.");
            return supplier;
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
