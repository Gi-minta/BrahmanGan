using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Equipos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 7: Equipos =====
public sealed class MaquinariaRepository : RepositoryBase<Maquinaria, MaquinariaId>, IMaquinariaRepository
{
    public MaquinariaRepository(ApplicationDbContext db) : base(db) { }
    public Task<Maquinaria?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(m => m.Codigo == codigo, ct);
    public async Task<IReadOnlyList<Maquinaria>> ListActivasAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(m => m.Estado != EstadoMaquinaria.BAJA).ToListAsync(ct);
}

public sealed class MantenimientoEquipoRepository : RepositoryBase<MantenimientoEquipo, MantenimientoEquipoId>, IMantenimientoEquipoRepository
{
    public MantenimientoEquipoRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<MantenimientoEquipo>> ListPorMaquinariaAsync(MaquinariaId idMaq, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(m => m.IdMaquinaria == idMaq).ToListAsync(ct);
}
