using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Pastoreo;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Pastoreo =====
public sealed class PlanPastoreoRepository : RepositoryBase<PlanPastoreo, PlanPastoreoId>, IPlanPastoreoRepository
{
    public PlanPastoreoRepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<PlanPastoreo>> ListPorPotreroAsync(PotreroId idPotrero, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.IdPotrero == idPotrero).ToListAsync(ct);

    public async Task<IReadOnlyList<PlanPastoreo>> ListActivosAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.Activo).ToListAsync(ct);
}
