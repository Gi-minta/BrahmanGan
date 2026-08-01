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
            .AddPolicy("SoloLectura",    p => p.RequireAuthenticatedUser());
        // Los permisos por módulo no se declaran como políticas: cada endpoint los exige
        // con Permissions("Modulo:Accion"), que FastEndpoints resuelve contra el claim
        // "permiso" (configurado en Program.cs). Mantener aquí una política por cada
        // combinación obligaría a declarar 84.

        return services;
    }
}
