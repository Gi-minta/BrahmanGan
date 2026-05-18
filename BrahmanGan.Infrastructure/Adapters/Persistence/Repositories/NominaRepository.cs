using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Nomina;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 8: Nómina =====
public sealed class TrabajadorRepository : RepositoryBase<Trabajador, TrabajadorId>, ITrabajadorRepository
{
    public TrabajadorRepository(ApplicationDbContext db) : base(db) { }
    public Task<Trabajador?> GetByCedulaAsync(string cedula, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(t => t.Cedula == cedula, ct);
    public async Task<IReadOnlyList<Trabajador>> ListActivosAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(t => t.Activo).ToListAsync(ct);
}

public sealed class PagoJornalRepository : RepositoryBase<PagoJornal, PagoJornalId>, IPagoJornalRepository
{
    public PagoJornalRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<PagoJornal>> ListPorTrabajadorAsync(TrabajadorId idTrabajador, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.IdTrabajador == idTrabajador).ToListAsync(ct);
    public async Task<IReadOnlyList<PagoJornal>> ListPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.Fecha >= desde && p.Fecha <= hasta).ToListAsync(ct);
}
