using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Application.Ports.Output;

public interface IAcumulacionInsumoPotreroRepository : IRepository<AcumulacionInsumoPotrero, AcumulacionInsumoPotreroId>
{
    Task<IReadOnlyList<AcumulacionInsumoPotrero>> ListByPotreroAsync(PotreroId idPotrero, CancellationToken ct = default);
}
