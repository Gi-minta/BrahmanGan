using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class SemenRepository : RepositoryBase<Semen, SemenId>, ISemenRepository
{
    public SemenRepository(ApplicationDbContext db) : base(db) { }

    public Task<Semen?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(s => s.Codigo == codigo, ct);

    public Task<IReadOnlyList<Semen>> ListActivosAsync(CancellationToken ct = default)
        => Set.AsNoTracking().Where(s => s.Activo).OrderBy(s => s.NombreToro)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<Semen>)t.Result, ct);
}
