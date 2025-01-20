using SupplierPortalAPI.Repositories;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Services
{
    public class SupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _supplierRepository.GetAllSuppliersAsync();
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            await _supplierRepository.AddSupplierAsync(supplier);
            await _supplierRepository.SaveChangesAsync();
        }
    }
}
