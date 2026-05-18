using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 7: Almacén =====
public sealed class InsumoRepository : RepositoryBase<Insumo, InsumoId>, IInsumoRepository
{
    public InsumoRepository(ApplicationDbContext db) : base(db) { }
    public Task<Insumo?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(i => i.Codigo == codigo, ct);
    public async Task<IReadOnlyList<Insumo>> ListActivosAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(i => i.Activo).ToListAsync(ct);
    public async Task<IReadOnlyList<Insumo>> ListBajoMinimoAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(i => i.Activo && i.StockActual < i.StockMinimo).ToListAsync(ct);
}

public sealed class KardexInsumoRepository : RepositoryBase<KardexInsumo, KardexInsumoId>, IKardexInsumoRepository
{
    public KardexInsumoRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<KardexInsumo>> ListPorInsumoAsync(InsumoId idInsumo, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(k => k.IdInsumo == idInsumo).ToListAsync(ct);
}
