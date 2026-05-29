using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services;

internal class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var secretKey = _configuration["JwtSettings:Secret"];
        
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("DETECTIVE: Ik kan JWT_SECRET helemaal niet vinden in de configuratie!");
        }
        if (secretKey.Length < 16)
        {
            throw new Exception($"DETECTIVE: Ik vind de sleutel wel, maar hij is te kort! Lengte is: {secretKey.Length}. Waarde is: '{secretKey}'");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId)
        };
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}