using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.Modulos.Seguridad;

/// <summary>
/// Permiso atómico sobre un módulo y una acción.
/// Clave lógica: Modulo + Accion (ej. "Inventario:Leer").
/// </summary>
public sealed class Permiso : Common.AggregateRoot<Common.PermisoId>
{
    public ModuloSistema Modulo { get; private set; }
    public AccionPermiso Accion { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;
    public bool Activo { get; private set; } = true;

    // Clave de permisos para claims ("Inventario:Leer")
    public string Clave => $"{Modulo}:{Accion}";

    private readonly List<RolPermiso> _rolesPermiso = [];
    public IReadOnlyCollection<RolPermiso> RolesPermiso => _rolesPermiso.AsReadOnly();

    // Auditoría (requerida)
    public DateTime CreatedAt { get; private set; }

    private Permiso() { }

    public static Permiso Crear(ModuloSistema modulo, AccionPermiso accion, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new BusinessRuleException("La descripción del permiso es requerida.");
        return new Permiso
        {
            Id = Common.PermisoId.New(),
            Modulo = modulo,
            Accion = accion,
            Descripcion = descripcion,
            Activo = true,
            CreatedAt = DateTime.UtcNow, // inicializar timestamp al crear instancia
        };
    }

    public void Desactivar() => Activo = false;
    public void Activar()   => Activo = true;
}
