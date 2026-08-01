using System.Security.Claims;
using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.API.Endpoints.Auth;

/// <summary>Payload que envía el frontend tras el flujo OAuth2.</summary>
public record OAuthCallbackRequest(
    string Email,
    string NombreCompleto,
    string IdExterno,
    string? IdToken = null);

/// <summary>Login con email y contraseña.</summary>
public sealed class LoginEndpoint(IAuthServicio auth) : Endpoint<LoginRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        => await Send.OkAsync(await auth.LoginAsync(req, ct), ct);
}

/// <summary>Registro de nuevo usuario.</summary>
public sealed class RegistrarUsuarioEndpoint(IAuthServicio auth) : Endpoint<RegistrarUsuarioRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("api/auth/registrar");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrarUsuarioRequest req, CancellationToken ct)
    {
        var result = await auth.RegistrarAsync(req, ct);
        await Send.ResponseAsync(result, 201, ct);
    }
}

/// <summary>Renovar el access token usando el refresh token.</summary>
public sealed class RefreshTokenEndpoint(IAuthServicio auth) : Endpoint<RefreshTokenRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("api/auth/refresh");
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
/// El cliente SPA realiza el flujo OAuth en el navegador y envía el ID token de Google
/// ya validado (o los datos del perfil). Para producción se debería validar el ID token
/// de Google con la clave pública de Google; aquí se acepta el payload del cliente.
/// </summary>
public sealed class LoginGoogleEndpoint(IAuthServicio auth) : Endpoint<OAuthCallbackRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("api/auth/oauth/google");
        AllowAnonymous();
    }

    public override async Task HandleAsync(OAuthCallbackRequest req, CancellationToken ct)
    {
        // En producción: verificar req.IdToken con Google APIs
        var result = await auth.LoginOAuthAsync(
            req.Email, req.NombreCompleto,
            ProveedorAuth.Google, req.IdExterno, ct);
        await Send.OkAsync(result, ct);
    }
}

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
