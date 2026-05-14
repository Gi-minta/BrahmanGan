using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Application.Ports.Output;

public interface IRolRepository
{
    Task<Rol?> ObtenerPorIdAsync(RolId id, CancellationToken ct = default);
    Task<Rol?> ObtenerPorNombreAsync(string nombre, CancellationToken ct = default);
    Task<IEnumerable<Rol>> ListarAsync(CancellationToken ct = default);
    Task<Rol?> ObtenerConPermisosAsync(RolId id, CancellationToken ct = default);
    Task AgregarAsync(Rol rol, CancellationToken ct = default);
    Task ActualizarAsync(Rol rol, CancellationToken ct = default);
}
