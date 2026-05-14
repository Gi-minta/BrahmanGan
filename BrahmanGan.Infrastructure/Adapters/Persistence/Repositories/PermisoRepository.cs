using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;
using BrahmanGan.Infrastructure.Adapters.Persistence;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ─────────────────────────────────────────────────────────────
//  PermisoRepository
// ─────────────────────────────────────────────────────────────
public sealed class PermisoRepository : IPermisoRepository
{
    private readonly ApplicationDbContext _db;
    public PermisoRepository(ApplicationDbContext db) => _db = db;

    public async Task<Permiso?> ObtenerPorIdAsync(PermisoId id, CancellationToken ct = default) =>
        await _db.Permisos.FindAsync([id], ct);

    public async Task<IEnumerable<Permiso>> ListarAsync(CancellationToken ct = default) =>
        await _db.Permisos.ToListAsync(ct);
}
