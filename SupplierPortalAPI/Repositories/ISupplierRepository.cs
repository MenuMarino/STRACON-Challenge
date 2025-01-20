using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<Supplier?> GetByIdAsync(int supplierId);
        Task AddSupplierAsync(Supplier supplier);
        Task SaveChangesAsync();
    }
}
