using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Application.Ports.Output;

public interface IComplementoRepository : IRepository<Complemento, ComplementoId>
{
    Task<IReadOnlyList<Complemento>> ListPorTratamientoAsync(HistorialCurativoId idTratamiento, CancellationToken ct = default);
}
