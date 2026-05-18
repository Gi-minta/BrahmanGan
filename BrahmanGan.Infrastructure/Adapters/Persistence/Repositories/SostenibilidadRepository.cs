using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sostenibilidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 8: Sostenibilidad =====
public sealed class CapturaCarbonoRepository : RepositoryBase<CapturaCarbono, CapturaCarbonoId>, ICapturaCarbonoRepository
{
    public CapturaCarbonoRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<CapturaCarbono>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(c => c.IdFinca == idFinca).ToListAsync(ct);
}

public sealed class ConsumoAguaRepository : RepositoryBase<ConsumoAgua, ConsumoAguaId>, IConsumoAguaRepository
{
    public ConsumoAguaRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<ConsumoAgua>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(c => c.IdFinca == idFinca).ToListAsync(ct);
}

public sealed class EventoMedioambientalRepository : RepositoryBase<EventoMedioambiental, EventoMedioambientalId>, IEventoMedioambientalRepository
{
    public EventoMedioambientalRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<EventoMedioambiental>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(e => e.IdFinca == idFinca).ToListAsync(ct);
}
