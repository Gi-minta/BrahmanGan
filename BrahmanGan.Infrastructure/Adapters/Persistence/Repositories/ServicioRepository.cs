using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class ServicioRepository : RepositoryBase<Servicio, ServicioId>, IServicioRepository
{
    public ServicioRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<Servicio>> ListByHembraAsync(AnimalId idHembra, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(s => s.IdHembra == idHembra).OrderByDescending(s => s.Fecha).ToListAsync(ct);
}
