using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class HistorialAnimalRepository : RepositoryBase<HistorialAnimal, HistorialAnimalId>, IHistorialAnimalRepository
{
    public HistorialAnimalRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<HistorialAnimal>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(h => h.IdAnimal == idAnimal).OrderByDescending(h => h.Fecha).ToListAsync(ct);
}
