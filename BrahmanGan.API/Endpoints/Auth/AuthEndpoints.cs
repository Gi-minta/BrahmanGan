using System.Security.Claims;
using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.API.Endpoints.Auth;

/// <summary>
/// Payload que envía el frontend tras el flujo OAuth2: únicamente el ID token que emite
/// Google. El correo, el nombre y el identificador de cuenta se leen del propio token una
/// vez validado; aceptarlos del cliente permitiría a cualquiera pedir un token a nombre de
/// quien quisiera.
/// </summary>
public record OAuthCallbackRequest(string IdToken);

/// <summary>Login con email y contraseña.</summary>
public sealed class LoginEndpoint(IAuthServicio auth) : Endpoint<LoginRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("api/auth/login");
        // Anónimo por necesidad: es el endpoint que emite el token. Exigir autenticación
        // aquí dejaría fuera a todo el mundo, sin forma de entrar.
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        => await Send.OkAsync(await auth.LoginAsync(req, ct), ct);
}

// El endpoint de registro público se retiró: permitía a cualquiera crearse una cuenta en
// la API. El alta de usuarios vive ahora en POST api/usuarios, dentro del módulo de
// Seguridad y reservada a administradores.

/// <summary>Renovar el access token usando el refresh token.</summary>
public sealed class RefreshTokenEndpoint(IAuthServicio auth) : Endpoint<RefreshTokenRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("api/auth/refresh");
        // Anónimo por necesidad: se invoca precisamente cuando el access token ya expiró.
        // La credencial que autoriza la operación es el refresh token del cuerpo.
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
        => await Send.OkAsync(await auth.RefreshTokenAsync(req, ct), ct);
}

/// <summary>Revocar el refresh token (cerrar sesión).</summary>
public sealed class LogoutEndpoint(IAuthServicio auth) : EndpointWithoutRequest
{
    public override void Configure() => Post("api/auth/logout");

    public override async Task HandleAsync(CancellationToken ct)
    {
        await auth.LogoutAsync(User.ObtenerUsuarioId(), ct);
        await Send.NoContentAsync(ct);
    }
}

/// <summary>Cambiar contraseña del usuario autenticado.</summary>
public sealed class CambiarPasswordEndpoint(IAuthServicio auth) : Endpoint<CambiarPasswordRequest>
{
    public override void Configure() => Post("api/auth/cambiar-password");

    public override async Task HandleAsync(CambiarPasswordRequest req, CancellationToken ct)
    {
        await auth.CambiarPasswordAsync(User.ObtenerUsuarioId(), req, ct);
        await Send.NoContentAsync(ct);
    }
}

/// <summary>
/// Login / registro vía Google OAuth2.
/// El cliente hace el flujo en el navegador con Google Identity Services y envía aquí el
/// ID token resultante. El token se valida contra Google —firma, emisor, caducidad y
/// audiencia— y la identidad se toma de él.
/// </summary>
public sealed class LoginGoogleEndpoint(IAuthServicio auth, IGoogleTokenValidator google)
    : Endpoint<OAuthCallbackRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("api/auth/oauth/google");
        // Anónimo por necesidad: es una vía de entrada alternativa al login, y quien la
        // usa todavía no tiene token. La credencial que autoriza es el ID token de Google,
        // que se verifica antes de emitir nada.
        AllowAnonymous();
    }

    public override async Task HandleAsync(OAuthCallbackRequest req, CancellationToken ct)
    {
        var identidad = await google.ValidarAsync(req.IdToken, ct);

        var result = await auth.LoginOAuthAsync(
            identidad.Email, identidad.NombreCompleto,
            ProveedorAuth.Google, identidad.IdExterno, ct);

        await Send.OkAsync(result, ct);
    }
}

/// <summary>
/// Indica al cliente si el login con Google está disponible y con qué ClientId.
/// El ClientId es público por definición —viaja en la URL de autorización de Google— y el
/// frontend lo necesita para inicializar Google Identity Services.
/// </summary>
public sealed class ConfiguracionOAuthEndpoint(IGoogleTokenValidator google, IConfiguration config)
    : EndpointWithoutRequest<OAuthConfigResponse>
{
    public override void Configure()
    {
        Get("api/auth/oauth/config");
        // Anónimo por necesidad: lo consulta la pantalla de login, antes de haber entrado.
        // No expone secretos: solo un booleano y el ClientId público.
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(
            new OAuthConfigResponse(
                google.EstaConfigurado,
                google.EstaConfigurado ? config["OAuth:Google:ClientId"]?.Trim() : null),
            ct);
}

/// <summary>Disponibilidad del login con Google y su ClientId público.</summary>
public record OAuthConfigResponse(bool GoogleHabilitado, string? GoogleClientId);

/// <summary>Perfil del usuario autenticado.</summary>
public sealed class MeEndpoint : EndpointWithoutRequest<UsuarioInfoResponse>
{
    public override void Configure() => Get("api/auth/me");

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id     = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? "0";
        var email  = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email") ?? "";
        var nombre = User.FindFirstValue(ClaimTypes.Name) ?? "";
        var roles  = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
        var perms  = User.FindAll("permiso").Select(c => c.Value).ToArray();

        await Send.OkAsync(new UsuarioInfoResponse(int.Parse(id), email, nombre, roles, perms), ct);
    }
}

internal static class ClaimsPrincipalExtensions
{
    public static int ObtenerUsuarioId(this ClaimsPrincipal user) =>
        int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("No se pudo obtener el ID del usuario."));
}
