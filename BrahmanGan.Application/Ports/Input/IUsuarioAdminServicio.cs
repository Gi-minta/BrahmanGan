using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

/// <summary>Servicio de administración de usuarios (vista admin).</summary>
public interface IUsuarioAdminServicio
{
    Task<IEnumerable<UsuarioResponse>> ListarUsuariosAsync(CancellationToken ct = default);
    Task<UsuarioResponse> ObtenerUsuarioAsync(int id, CancellationToken ct = default);
    Task AsignarRolAsync(AsignarRolUsuarioRequest request, CancellationToken ct = default);
    Task RevocarRolAsync(AsignarRolUsuarioRequest request, CancellationToken ct = default);
    Task DesactivarUsuarioAsync(int id, CancellationToken ct = default);
    Task ActivarUsuarioAsync(int id, CancellationToken ct = default);
}
