using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;
public sealed class CotizacionVentaRepository : RepositoryBase<CotizacionVenta, CotizacionVentaId>, ICotizacionVentaRepository
{
    public CotizacionVentaRepository(ApplicationDbContext db) : base(db) { }
    public override async Task<CotizacionVenta?> GetByIdAsync(CotizacionVentaId id, CancellationToken ct = default)
        => await Set.Include(c => c.Detalles).FirstOrDefaultAsync(c => c.Id == id, ct);
}
