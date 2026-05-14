using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 5 =====
public sealed class ControlLecheAnimalRepository : RepositoryBase<ControlLecheAnimal, ControlLecheAnimalId>, IControlLecheAnimalRepository
{
    public ControlLecheAnimalRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<ControlLecheAnimal>> ListByAnimalAsync(AnimalId idAnimal, DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => await Set.AsNoTracking()
            .Where(c => c.IdAnimal == idAnimal && c.Fecha >= desde && c.Fecha <= hasta)
            .OrderBy(c => c.Fecha).ToListAsync(ct);
}
