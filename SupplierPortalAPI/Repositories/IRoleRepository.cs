using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
