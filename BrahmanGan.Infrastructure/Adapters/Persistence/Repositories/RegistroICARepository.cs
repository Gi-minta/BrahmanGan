using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Trazabilidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 7: Trazabilidad =====
public sealed class RegistroICARepository : RepositoryBase<RegistroICA, RegistroICAId>, IRegistroICARepository
{
    public RegistroICARepository(ApplicationDbContext db) : base(db) { }

    public async Task<IReadOnlyList<RegistroICA>> ListPorAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(r => r.IdAnimal == idAnimal).ToListAsync(ct);

    public async Task<IReadOnlyList<RegistroICA>> ListProximosVencerAsync(DateOnly hoy, int diasUmbral, CancellationToken ct = default)
        => await Set.AsNoTracking()
            .Where(r => r.Estado == EstadoRegistroICA.VIGENTE
                && r.FechaVencimiento.HasValue
                && r.FechaVencimiento!.Value.DayNumber - hoy.DayNumber <= diasUmbral)
            .ToListAsync(ct);
}
