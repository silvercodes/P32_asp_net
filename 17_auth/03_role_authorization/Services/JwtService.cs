using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using _03_role_authorization.Models;
using Microsoft.IdentityModel.Tokens;

namespace _03_role_authorization.Services;

public class JwtService
{
    private readonly IConfiguration config;
    public JwtService(IConfiguration config)
    {
        this.config = config;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (string role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
