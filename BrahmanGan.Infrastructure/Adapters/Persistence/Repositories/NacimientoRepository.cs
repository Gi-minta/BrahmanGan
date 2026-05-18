using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class NacimientoRepository : RepositoryBase<Nacimiento, NacimientoId>, INacimientoRepository
{
    public NacimientoRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<Nacimiento>> ListByGestacionAsync(GestacionId idGestacion, CancellationToken ct = default)
        => Set.AsNoTracking().Where(n => n.IdGestacion == idGestacion).OrderBy(n => n.Fecha)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<Nacimiento>)t.Result, ct);
}
