using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Application.Ports.Output;

public interface IProduccionLecheRepository : IRepository<ProduccionLeche, ProduccionLecheId>
{
    Task<ProduccionLeche?> GetByFincaFechaAsync(FincaId idFinca, DateOnly fecha, CancellationToken ct = default);
}
