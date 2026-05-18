using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class HistorialPreventivoRepository : RepositoryBase<HistorialPreventivo, HistorialPreventivoId>, IHistorialPreventivoRepository
{
    public HistorialPreventivoRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<HistorialPreventivo>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => Set.AsNoTracking().Where(h => h.IdAnimal == idAnimal).OrderByDescending(h => h.Fecha)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<HistorialPreventivo>)t.Result, ct);
}
