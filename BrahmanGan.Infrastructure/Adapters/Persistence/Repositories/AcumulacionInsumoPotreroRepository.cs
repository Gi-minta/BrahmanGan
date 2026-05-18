using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class AcumulacionInsumoPotreroRepository : RepositoryBase<AcumulacionInsumoPotrero, AcumulacionInsumoPotreroId>, IAcumulacionInsumoPotreroRepository
{
    public AcumulacionInsumoPotreroRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<AcumulacionInsumoPotrero>> ListByPotreroAsync(PotreroId idPotrero, CancellationToken ct = default)
        => Set.AsNoTracking().Where(a => a.IdPotrero == idPotrero).OrderByDescending(a => a.Fecha)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<AcumulacionInsumoPotrero>)t.Result, ct);
}
