using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Alimentacion;

namespace BrahmanGan.Application.Ports.Output;

public interface IPlanAlimentacionRepository : IRepository<PlanAlimentacion, PlanAlimentacionId>
{
    Task<IReadOnlyList<PlanAlimentacion>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default);
    Task<IReadOnlyList<PlanAlimentacion>> ListActivosAsync(CancellationToken ct = default);
}

public interface IDetallePlanAlimentacionRepository : IRepository<DetallePlanAlimentacion, DetallePlanAlimentacionId>
{
    Task<IReadOnlyList<DetallePlanAlimentacion>> ListPorPlanAsync(PlanAlimentacionId idPlan, CancellationToken ct = default);
}
