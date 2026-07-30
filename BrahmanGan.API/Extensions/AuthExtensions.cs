using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BrahmanGan.API.Extensions;

/// <summary>
/// Configura autenticación JWT + OAuth2 (Google) en la aplicación.
/// </summary>
public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration config)
    {
        var secretKey = config["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Jwt:SecretKey no configurada en appsettings.");
        var issuer    = config["Jwt:Issuer"]   ?? "BrahmanGan";
        var audience  = config["Jwt:Audience"] ?? "BrahmanGanClient";

        var authBuilder = services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidIssuer              = issuer,
                    ValidateAudience         = true,
                    ValidAudience            = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime         = true,
                    ClockSkew                = TimeSpan.FromSeconds(30),
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        if (ctx.Exception is SecurityTokenExpiredException)
                            ctx.Response.Headers["Token-Expired"] = "true";
                        return Task.CompletedTask;
                    }
                };
            });

        // OAuth2 con Google: se registra SOLO si hay credenciales configuradas. Sin ellas,
        // el login con Google queda deshabilitado y la app arranca igual (AddGoogle valida
        // que ClientId/ClientSecret no estén vacíos y, de lo contrario, falla al arrancar).
        // Configúralas de forma segura: OAuth__Google__ClientId / OAuth__Google__ClientSecret,
        // user-secrets o un appsettings NO versionado.
        var googleClientId     = config["OAuth:Google:ClientId"];
        var googleClientSecret = config["OAuth:Google:ClientSecret"];
        if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
        {
            authBuilder.AddGoogle(options =>
            {
                options.ClientId     = googleClientId;
                options.ClientSecret = googleClientSecret;
                // El callback estándar es /signin-google pero en SPA usaremos el flujo "token" manual.
                options.SaveTokens   = true;
            });
        }

        // Políticas de autorización basadas en roles
        services.AddAuthorizationBuilder()
            .AddPolicy("Administrador",  p => p.RequireRole("Administrador"))
            .AddPolicy("Gerente",        p => p.RequireRole("Administrador", "Gerente"))
            .AddPolicy("Veterinario",    p => p.RequireRole("Administrador", "Gerente", "Veterinario"))
            .AddPolicy("Operador",       p => p.RequireRole("Administrador", "Gerente", "Veterinario", "Operador"))
            .AddPolicy("SoloLectura",    p => p.RequireAuthenticatedUser())
            // Políticas basadas en permisos custom claim
            .AddPolicy("perm:animales:crear",   p => p.RequireClaim("permiso", "Inventario:Crear"))
            .AddPolicy("perm:animales:editar",  p => p.RequireClaim("permiso", "Inventario:Editar"))
            .AddPolicy("perm:animales:eliminar",p => p.RequireClaim("permiso", "Inventario:Eliminar"))
            .AddPolicy("perm:reportes",         p => p.RequireClaim("permiso", "Reportes:Exportar"));

        return services;
    }
}
