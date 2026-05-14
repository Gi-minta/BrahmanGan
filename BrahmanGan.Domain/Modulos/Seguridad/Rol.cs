using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Seguridad;

/// <summary>
/// Rol del sistema. Agrupa permisos y se asigna a usuarios.
/// Roles predefinidos: Administrador, Veterinario, Operador, Gerente, Auditor.
/// </summary>
public sealed class Rol : AggregateRoot<RolId>
{
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public bool EsSistema { get; private set; }
    public bool Activo { get; private set; } = true;

    private readonly List<RolPermiso> _rolesPermiso = [];
    public IReadOnlyCollection<RolPermiso> RolesPermiso => _rolesPermiso.AsReadOnly();

    private readonly List<UsuarioRol> _usuariosRol = [];
    public IReadOnlyCollection<UsuarioRol> UsuariosRol => _usuariosRol.AsReadOnly();

    private Rol() { }

    public static Rol Crear(string nombre, string descripcion, bool esSistema = false)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new BusinessRuleException("El nombre del rol es requerido.");
        return new Rol
        {
            Id = RolId.New(),
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            EsSistema = esSistema,
            Activo = true,
        };
    }

    public void AsignarPermiso(Permiso permiso)
    {
        if (_rolesPermiso.Any(rp => rp.PermisoId == permiso.Id)) return;
        _rolesPermiso.Add(new RolPermiso(this.Id, permiso.Id));
    }

    public void RevocarPermiso(PermisoId permisoId)
    {
        var rp = _rolesPermiso.FirstOrDefault(r => r.PermisoId == permisoId);
        if (rp is not null) _rolesPermiso.Remove(rp);
    }

    public void Actualizar(string nombre, string descripcion)
    {
        if (EsSistema) throw new BusinessRuleException("Los roles del sistema no pueden modificarse.");
        Nombre = nombre.Trim();
        Descripcion = descripcion;
    }

    public void Desactivar()
    {
        if (EsSistema) throw new BusinessRuleException("Los roles del sistema no pueden desactivarse.");
        Activo = false;
    }
}
