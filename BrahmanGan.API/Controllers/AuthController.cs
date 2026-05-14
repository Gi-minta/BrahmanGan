using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.API.Controllers;

/// <summary>
/// Autenticación y gestión de tokens JWT.
/// Soporta login local (email/password) y OAuth2 (Google).
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthServicio _auth;

    public AuthController(IAuthServicio auth) => _auth = auth;

    // ─── POST /api/auth/login ─────────────────────────────────
    /// <summary>Login con email y contraseña.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<TokenResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        var result = await _auth.LoginAsync(request, ct);
        return Ok(result);
    }

    // ─── POST /api/auth/registrar ─────────────────────────────
    /// <summary>Registro de nuevo usuario.</summary>
    [HttpPost("registrar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TokenResponse>> Registrar(
        [FromBody] RegistrarUsuarioRequest request,
        CancellationToken ct)
    {
        var result = await _auth.RegistrarAsync(request, ct);
        return StatusCode(201, result);
    }

    // ─── POST /api/auth/refresh ───────────────────────────────
    /// <summary>Renovar el access token usando el refresh token.</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<TokenResponse>> Refresh(
        [FromBody] RefreshTokenRequest request,
        CancellationToken ct)
    {
        var result = await _auth.RefreshTokenAsync(request, ct);
        return Ok(result);
    }

    // ─── POST /api/auth/logout ────────────────────────────────
    /// <summary>Revocar el refresh token (cerrar sesión).</summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var userId = ObtenerUsuarioId();
        await _auth.LogoutAsync(userId, ct);
        return NoContent();
    }

    // ─── POST /api/auth/cambiar-password ──────────────────────
    /// <summary>Cambiar contraseña del usuario autenticado.</summary>
    [HttpPost("cambiar-password")]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CambiarPassword(
        [FromBody] CambiarPasswordRequest request,
        CancellationToken ct)
    {
        var userId = ObtenerUsuarioId();
        await _auth.CambiarPasswordAsync(userId, request, ct);
        return NoContent();
    }

    // ─── POST /api/auth/oauth/google ──────────────────────────
    /// <summary>
    /// Login / registro vía Google OAuth2.
    /// El cliente SPA realiza el flujo OAuth en el navegador y envía el ID token de Google
    /// ya validado (o los datos del perfil). Para producción se debería validar el ID token
    /// de Google con la clave pública de Google; aquí se acepta el payload del cliente.
    /// </summary>
    [HttpPost("oauth/google")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TokenResponse>> LoginGoogle(
        [FromBody] OAuthCallbackRequest request,
        CancellationToken ct)
    {
        // En producción: verificar request.IdToken con Google APIs
        var result = await _auth.LoginOAuthAsync(
            request.Email, request.NombreCompleto,
            ProveedorAuth.Google, request.IdExterno, ct);
        return Ok(result);
    }

    // ─── GET /api/auth/me ────────────────────────────────────
    /// <summary>Perfil del usuario autenticado.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioInfoResponse), 200)]
    public IActionResult Me()
    {
        var id     = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? "0";
        var email  = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email") ?? "";
        var nombre = User.FindFirstValue(ClaimTypes.Name) ?? "";
        var roles  = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
        var perms  = User.FindAll("permiso").Select(c => c.Value).ToArray();

        return Ok(new UsuarioInfoResponse(int.Parse(id), email, nombre, roles, perms));
    }

    // ─── Helper ──────────────────────────────────────────────
    private int ObtenerUsuarioId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("No se pudo obtener el ID del usuario."));
}
