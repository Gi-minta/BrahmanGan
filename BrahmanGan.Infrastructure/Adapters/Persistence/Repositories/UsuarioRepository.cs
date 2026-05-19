using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ─────────────────────────────────────────────────────────────
//  UsuarioRepository
// ─────────────────────────────────────────────────────────────
public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _db;
    public UsuarioRepository(ApplicationDbContext db) => _db = db;

    public async Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default) =>
        await _db.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim(), ct);

    public async Task<Usuario?> ObtenerPorIdAsync(UsuarioId id, CancellationToken ct = default) =>
        await _db.Usuarios.FindAsync([id], ct);

    public async Task<Usuario?> ObtenerPorIdExternoAsync(string idExterno, ProveedorAuth proveedor, CancellationToken ct = default) =>
        await _db.Usuarios
            .FirstOrDefaultAsync(u => u.IdExterno == idExterno && u.Proveedor == proveedor, ct);

    public async Task<IEnumerable<Usuario>> ListarAsync(CancellationToken ct = default) =>
        await _db.Usuarios
            .Include(u => u.UsuariosRol).ThenInclude(ur => ur.Rol)
            .ToListAsync(ct);

    public async Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default) =>
        await _db.Usuarios.AnyAsync(u => u.Email == email.ToLowerInvariant().Trim(), ct);

    public async Task AgregarAsync(Usuario usuario, CancellationToken ct = default) =>
        await _db.Usuarios.AddAsync(usuario, ct);

    public Task ActualizarAsync(Usuario usuario, CancellationToken ct = default)
    {
        _db.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task<Usuario?> ObtenerConRolesAsync(UsuarioId id, CancellationToken ct = default) =>
        await _db.Usuarios
            .AsSplitQuery()
            .Include(u => u.UsuariosRol)
                .ThenInclude(ur => ur.Rol)
                    .ThenInclude(r => r!.RolesPermiso)
                        .ThenInclude(rp => rp.Permiso)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<Usuario?> ObtenerConRolesPorEmailAsync(string email, CancellationToken ct = default) =>
        await _db.Usuarios
            .AsSplitQuery()
            .Include(u => u.UsuariosRol)
                .ThenInclude(ur => ur.Rol)
                    .ThenInclude(r => r!.RolesPermiso)
                        .ThenInclude(rp => rp.Permiso)
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim(), ct);
}
