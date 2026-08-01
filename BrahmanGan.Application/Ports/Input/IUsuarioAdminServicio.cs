using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

/// <summary>Servicio de administración de usuarios (vista admin).</summary>
public interface IUsuarioAdminServicio
{
    Task<IEnumerable<UsuarioResponse>> ListarUsuariosAsync(CancellationToken ct = default);
    Task<UsuarioResponse> ObtenerUsuarioAsync(int id, CancellationToken ct = default);

    /// <summary>Da de alta un usuario con contraseña temporal y un rol inicial.</summary>
    Task<UsuarioResponse> CrearUsuarioAsync(CrearUsuarioAdminRequest request, CancellationToken ct = default);

    /// <summary>
    /// Fija una contraseña temporal a un usuario existente, para devolverle el acceso.
    /// </summary>
    Task RestablecerPasswordAsync(int id, RestablecerPasswordAdminRequest request, CancellationToken ct = default);
    Task AsignarRolAsync(AsignarRolUsuarioRequest request, CancellationToken ct = default);
    Task RevocarRolAsync(AsignarRolUsuarioRequest request, CancellationToken ct = default);
    Task DesactivarUsuarioAsync(int id, CancellationToken ct = default);
    Task ActivarUsuarioAsync(int id, CancellationToken ct = default);
}
