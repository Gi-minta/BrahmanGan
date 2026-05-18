using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class MovimientoAnimalRepository : RepositoryBase<MovimientoAnimal, MovimientoAnimalId>, IMovimientoAnimalRepository
{
    public MovimientoAnimalRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<MovimientoAnimal>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => Set.AsNoTracking().Where(m => m.IdAnimal == idAnimal).OrderByDescending(m => m.Fecha)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<MovimientoAnimal>)t.Result, ct);
}
