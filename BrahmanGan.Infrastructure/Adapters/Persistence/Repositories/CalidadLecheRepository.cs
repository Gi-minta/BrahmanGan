using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class CalidadLecheRepository : RepositoryBase<CalidadLeche, CalidadLecheId>, ICalidadLecheRepository
{
    public CalidadLecheRepository(ApplicationDbContext db) : base(db) { }

    public Task<IReadOnlyList<CalidadLeche>> ListPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => Set.AsNoTracking().Where(c => c.Fecha >= desde && c.Fecha <= hasta).OrderByDescending(c => c.Fecha)
              .ToListAsync(ct).ContinueWith(t => (IReadOnlyList<CalidadLeche>)t.Result, ct);
}
