using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ─────────────────────────────────────────────────────────────
//  RolRepository
// ─────────────────────────────────────────────────────────────
public sealed class RolRepository : IRolRepository
{
    private readonly ApplicationDbContext _db;
    public RolRepository(ApplicationDbContext db) => _db = db;

    public async Task<Rol?> ObtenerPorIdAsync(RolId id, CancellationToken ct = default) =>
        await _db.Roles.FindAsync([id], ct);

    public async Task<Rol?> ObtenerPorNombreAsync(string nombre, CancellationToken ct = default) =>
        await _db.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre, ct);

    public async Task<IEnumerable<Rol>> ListarAsync(CancellationToken ct = default) =>
        await _db.Roles.Include(r => r.RolesPermiso).ThenInclude(rp => rp.Permiso).ToListAsync(ct);

    public async Task<Rol?> ObtenerConPermisosAsync(RolId id, CancellationToken ct = default) =>
        await _db.Roles
            .Include(r => r.RolesPermiso).ThenInclude(rp => rp.Permiso)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task AgregarAsync(Rol rol, CancellationToken ct = default) =>
        await _db.Roles.AddAsync(rol, ct);

    public Task ActualizarAsync(Rol rol, CancellationToken ct = default)
    {
        _db.Roles.Update(rol);
        return Task.CompletedTask;
    }
}
