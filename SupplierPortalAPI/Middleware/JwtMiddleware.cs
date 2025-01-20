using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SupplierPortalAPI.Configuration;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtSettings _jwtSettings;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration, JwtSettings jwtSettings)
    {
        _next = next;
        _jwtSettings = jwtSettings;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Cookies["jwt"];

        if (!string.IsNullOrEmpty(token))
        {
            var claimsPrincipal = ValidateJwtToken(token);
            if (claimsPrincipal != null)
            {
                context.User = claimsPrincipal;
            }
        }
        await _next(context);
    }

    private ClaimsPrincipal? ValidateJwtToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtSettings.Issuer, 
                ValidAudience = _jwtSettings.Audience, 
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
