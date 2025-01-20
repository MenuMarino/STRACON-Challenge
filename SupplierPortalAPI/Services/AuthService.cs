using System.Security.Cryptography;
using System.Text;
using SupplierPortalAPI.DTOs;
using SupplierPortalAPI.Models;
using SupplierPortalAPI.Repositories;

namespace SupplierPortalAPI.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return false;
            }

            var role = await _roleRepository.GetRoleByIdAsync(registerDto.RoleId) ?? throw new ArgumentException("Rol no válido proporcionado.");
            
            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = HashPassword(registerDto.Password),
                Role = role
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<User?> ValidateUserAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            if (user == null || user.PasswordHash != HashPassword(loginDto.Password))
            {
                return null;
            }

            return user;
        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
