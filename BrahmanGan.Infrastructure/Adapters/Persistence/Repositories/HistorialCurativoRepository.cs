using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class HistorialCurativoRepository : RepositoryBase<HistorialCurativo, HistorialCurativoId>, IHistorialCurativoRepository
{
    public HistorialCurativoRepository(ApplicationDbContext db) : base(db) { }
    public override async Task<HistorialCurativo?> GetByIdAsync(HistorialCurativoId id, CancellationToken ct = default)
        => await Set.Include(h => h.Detalles).FirstOrDefaultAsync(h => h.Id == id, ct);
}
