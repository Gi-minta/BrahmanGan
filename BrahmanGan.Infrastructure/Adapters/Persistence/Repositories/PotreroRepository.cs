using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class PotreroRepository : RepositoryBase<Potrero, PotreroId>, IPotreroRepository
{
    public PotreroRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<Potrero>> ListByFincaAsync(FincaId idFinca, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(p => p.IdFinca == idFinca).ToListAsync(ct);
}
