using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.Modulos.Seguridad;

/// <summary>Tabla de unión Rol ↔ Permiso.</summary>
public sealed class RolPermiso
{
    public RolId RolId { get; }
    public PermisoId PermisoId { get; }
    public Permiso? Permiso { get; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    private RolPermiso() { RolId = RolId.New(); PermisoId = PermisoId.New(); }
    public RolPermiso(RolId rolId, PermisoId permisoId) { RolId = rolId; PermisoId = permisoId; }
}
