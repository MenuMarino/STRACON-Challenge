using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SupplierPortalAPI.DTOs;
using SupplierPortalAPI.Services;
using SupplierPortalAPI.Configuration;
using SupplierPortalAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace SupplierPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(AuthService authService, JwtSettings jwtSettings)
        {
            _authService = authService;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var success = await _authService.RegisterAsync(registerDto);
            if (!success)
            {
                return BadRequest(new { message = "Un usuario con este nombre ya existe." });
            }

            return Ok(new { message = "El usuario se ha registrado correctamente." });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _authService.ValidateUserAsync(loginDto);
            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            var token = GenerateJwtToken(user);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Require HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new { message = "Login successful", role = user.Role.Name });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("ValidateToken")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            var role = roleClaim?.Value;

            if (string.IsNullOrEmpty(role))
            {
                return Unauthorized(new { message = "Role is missing or invalid." });
            }

            return Ok(new { role });
        }

    }
}
