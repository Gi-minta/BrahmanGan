using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Application.Ports.Output;

public interface IServicioRepository : IRepository<Servicio, ServicioId>
{
    Task<IReadOnlyList<Servicio>> ListByHembraAsync(AnimalId idHembra, CancellationToken ct = default);
}
