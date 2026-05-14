using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

/// <summary>Servicio de administración de roles y permisos.</summary>
public interface IRolServicio
{
    Task<IEnumerable<RolResponse>> ListarRolesAsync(CancellationToken ct = default);
    Task<RolResponse> ObtenerRolAsync(int id, CancellationToken ct = default);
    Task<RolResponse> CrearRolAsync(CrearRolRequest request, CancellationToken ct = default);
    Task AsignarPermisoAsync(AsignarPermisoRolRequest request, CancellationToken ct = default);
    Task RevocarPermisoAsync(AsignarPermisoRolRequest request, CancellationToken ct = default);
}
