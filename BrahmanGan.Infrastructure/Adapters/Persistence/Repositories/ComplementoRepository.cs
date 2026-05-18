using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class ComplementoRepository : RepositoryBase<Complemento, ComplementoId>, IComplementoRepository
{
    public ComplementoRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<Complemento>> ListPorTratamientoAsync(HistorialCurativoId idTratamiento, CancellationToken ct = default)
        => Set.AsNoTracking().Where(c => c.IdTratamiento == idTratamiento).OrderBy(c => c.Fecha)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<Complemento>)t.Result, ct);
}
