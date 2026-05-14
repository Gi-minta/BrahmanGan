using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Application.Ports.Output;

public interface IPotreroRepository : IRepository<Potrero, PotreroId>
{
    Task<IReadOnlyList<Potrero>> ListByFincaAsync(FincaId idFinca, CancellationToken ct = default);
}
