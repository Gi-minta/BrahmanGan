using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class ParametroLactanciaRepository : RepositoryBase<ParametroLactancia, ParametroLactanciaId>, IParametroLactanciaRepository
{
    public ParametroLactanciaRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<ParametroLactancia>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => Set.AsNoTracking().Where(p => p.IdAnimal == idAnimal).OrderBy(p => p.NumeroParto)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<ParametroLactancia>)t.Result, ct);
}
