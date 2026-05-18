using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Pastoreo;

namespace BrahmanGan.Application.Ports.Output;

public interface IPlanPastoreoRepository : IRepository<PlanPastoreo, PlanPastoreoId>
{
    Task<IReadOnlyList<PlanPastoreo>> ListPorPotreroAsync(PotreroId idPotrero, CancellationToken ct = default);
    Task<IReadOnlyList<PlanPastoreo>> ListActivosAsync(CancellationToken ct = default);
}
