using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Application.Ports.Output;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default);
    Task<Usuario?> ObtenerPorIdAsync(UsuarioId id, CancellationToken ct = default);
    Task<Usuario?> ObtenerPorIdExternoAsync(string idExterno, ProveedorAuth proveedor, CancellationToken ct = default);
    Task<IEnumerable<Usuario>> ListarAsync(CancellationToken ct = default);
    Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default);
    Task AgregarAsync(Usuario usuario, CancellationToken ct = default);
    Task ActualizarAsync(Usuario usuario, CancellationToken ct = default);
    /// <summary>Carga un usuario con sus roles y permisos.</summary>
    Task<Usuario?> ObtenerConRolesAsync(UsuarioId id, CancellationToken ct = default);
    /// <summary>Carga un usuario por email junto con roles y permisos en una sola consulta dividida.</summary>
    Task<Usuario?> ObtenerConRolesPorEmailAsync(string email, CancellationToken ct = default);
}
