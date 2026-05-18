using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Alimentacion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Alimentación =====
public sealed class PlanAlimentacionRepository : RepositoryBase<PlanAlimentacion, PlanAlimentacionId>, IPlanAlimentacionRepository
{
    public PlanAlimentacionRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<PlanAlimentacion>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.IdFinca == idFinca).ToListAsync(ct);

    public async Task<IReadOnlyList<PlanAlimentacion>> ListActivosAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.Activo).ToListAsync(ct);
}

public sealed class DetallePlanAlimentacionRepository : RepositoryBase<DetallePlanAlimentacion, DetallePlanAlimentacionId>, IDetallePlanAlimentacionRepository
{
    public DetallePlanAlimentacionRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<DetallePlanAlimentacion>> ListPorPlanAsync(PlanAlimentacionId idPlan, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(d => d.IdPlan == idPlan).ToListAsync(ct);
}
