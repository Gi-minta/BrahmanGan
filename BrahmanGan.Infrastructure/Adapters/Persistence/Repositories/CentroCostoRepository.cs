using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 7: Costos =====
public sealed class CentroCostoRepository : RepositoryBase<CentroCosto, CentroCostoId>, ICentroCostoRepository
{
    public CentroCostoRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<CentroCosto>> ListActivosAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(c => c.Activo).ToListAsync(ct);
}

public sealed class GastoGeneralRepository : RepositoryBase<GastoGeneral, GastoGeneralId>, IGastoGeneralRepository
{
    public GastoGeneralRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<GastoGeneral>> ListPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(g => g.Fecha >= desde && g.Fecha <= hasta).ToListAsync(ct);
}

public sealed class IngresoRepository : RepositoryBase<Ingreso, IngresoId>, IIngresoRepository
{
    public IngresoRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<Ingreso>> ListPorCentroAsync(CentroCostoId idCentro, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(i => i.IdCentro == idCentro).ToListAsync(ct);
}
