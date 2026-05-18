using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class GrupoManejoRepository : RepositoryBase<GrupoManejo, GrupoManejoId>, IGrupoManejoRepository
{
    public GrupoManejoRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<GrupoManejo>> ListActivosAsync(CancellationToken ct = default)
        => Set.AsNoTracking().Where(g => g.Activo).OrderBy(g => g.Nombre)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<GrupoManejo>)t.Result, ct);
}
