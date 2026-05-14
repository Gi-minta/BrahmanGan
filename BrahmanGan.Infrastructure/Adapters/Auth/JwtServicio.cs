using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BrahmanGan.Infrastructure.Adapters.Auth;

/// <summary>
/// Generación y validación de JWT.
/// Configuración requerida en appsettings.json:
/// <code>
/// "Jwt": { "SecretKey": "...", "Issuer": "BrahmanGan", "Audience": "BrahmanGanClient", "ExpiresMinutes": 60 }
/// </code>
/// </summary>
public sealed class JwtServicio : IJwtServicio
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiresMinutes;

    public JwtServicio(IConfiguration config)
    {
        _secretKey      = config["Jwt:SecretKey"]      ?? throw new InvalidOperationException("Jwt:SecretKey no configurada.");
        _issuer         = config["Jwt:Issuer"]         ?? "BrahmanGan";
        _audience       = config["Jwt:Audience"]       ?? "BrahmanGanClient";
        _expiresMinutes = int.TryParse(config["Jwt:ExpiresMinutes"], out var m) ? m : 60;
    }

    // ─── Generar Access Token ─────────────────────────────────
    public string GenerarAccessToken(UsuarioInfoResponse info)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   info.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, info.Email),
            new(ClaimTypes.Name,               info.NombreCompleto),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
        };

        // Roles como múltiples claims "role"
        foreach (var rol in info.Roles)
            claims.Add(new Claim(ClaimTypes.Role, rol));

        // Permisos como múltiples claims "permiso"
        foreach (var permiso in info.Permisos)
            claims.Add(new Claim("permiso", permiso));

        var token = new JwtSecurityToken(
            issuer:             _issuer,
            audience:           _audience,
            claims:             claims,
            notBefore:          DateTime.UtcNow,
            expires:            DateTime.UtcNow.AddMinutes(_expiresMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ─── Generar Refresh Token ────────────────────────────────
    public string GenerarRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    // ─── Validar / leer Access Token expirado ────────────────
    public UsuarioInfoResponse? ValidarAccessToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // Validamos firma e issuer pero NO la expiración (para refresh)
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidIssuer              = _issuer,
                ValidateAudience         = true,
                ValidAudience            = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = key,
                ValidateLifetime         = false,  // expirado OK en refresh
            }, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwt) return null;

            var id     = int.Parse(jwt.Subject);
            var email  = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? "";
            var nombre = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "";
            var roles  = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            var perms  = jwt.Claims.Where(c => c.Type == "permiso").Select(c => c.Value).ToArray();

            return new UsuarioInfoResponse(id, email, nombre, roles, perms);
        }
        catch
        {
            return null;
        }
    }
}
