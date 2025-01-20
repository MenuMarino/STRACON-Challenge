using Microsoft.EntityFrameworkCore;
using SupplierPortalAPI.Data;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SupplierPortalContext _context;

        public UserRepository(SupplierPortalContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
