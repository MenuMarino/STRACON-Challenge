using Microsoft.EntityFrameworkCore;
using SupplierPortalAPI.Data;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SupplierPortalContext _context;

        public RoleRepository(SupplierPortalContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
