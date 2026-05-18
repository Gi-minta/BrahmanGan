using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Application.Ports.Output;

// Fase 3
public interface ISemenRepository : IRepository<Semen, SemenId>
{
    Task<Semen?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    Task<IReadOnlyList<Semen>> ListActivosAsync(CancellationToken ct = default);
}
