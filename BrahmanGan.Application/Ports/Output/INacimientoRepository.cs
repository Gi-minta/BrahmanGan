using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Application.Ports.Output;

public interface INacimientoRepository : IRepository<Nacimiento, NacimientoId>
{
    Task<IReadOnlyList<Nacimiento>> ListByGestacionAsync(GestacionId idGestacion, CancellationToken ct = default);
}
