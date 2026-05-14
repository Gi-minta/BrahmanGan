using BrahmanGan.Application.DTOs;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Application.Ports.Input;

/// <summary>Servicio de autenticación: login, registro, refresh, OAuth2.</summary>
public interface IAuthServicio
{
    Task<TokenResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<TokenResponse> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
    Task<TokenResponse> LoginOAuthAsync(string email, string nombreCompleto,
                                         ProveedorAuth proveedor, string idExterno,
                                         CancellationToken ct = default);
    Task LogoutAsync(int usuarioId, CancellationToken ct = default);
    Task CambiarPasswordAsync(int usuarioId, CambiarPasswordRequest request, CancellationToken ct = default);
}
