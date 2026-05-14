using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Application.Ports.Output;

public interface IPermisoRepository
{
    Task<Permiso?> ObtenerPorIdAsync(PermisoId id, CancellationToken ct = default);
    Task<IEnumerable<Permiso>> ListarAsync(CancellationToken ct = default);
}
