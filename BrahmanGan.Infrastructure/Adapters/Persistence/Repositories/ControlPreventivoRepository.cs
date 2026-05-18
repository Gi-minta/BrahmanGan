using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class ControlPreventivoRepository : RepositoryBase<ControlPreventivo, ControlPreventivoId>, IControlPreventivoRepository
{
    public ControlPreventivoRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<ControlPreventivo>> ListAllAsync(CancellationToken ct = default)
        => Set.AsNoTracking().Where(c => c.Activo).OrderBy(c => c.Nombre)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<ControlPreventivo>)t.Result, ct);
}
