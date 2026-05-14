using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class PesajeRepository : RepositoryBase<Pesaje, PesajeId>, IPesajeRepository
{
    public PesajeRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<Pesaje>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.IdAnimal == idAnimal).OrderBy(p => p.Fecha).ToListAsync(ct);
}
