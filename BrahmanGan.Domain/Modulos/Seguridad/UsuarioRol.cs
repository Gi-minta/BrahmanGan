using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;
using Common;

namespace BrahmanGan.Domain.Modulos.Seguridad;

public sealed class UsuarioRol : Entity<UsuarioRolId>
{
    // Propiedades del valor (FKs)
    public UsuarioId UsuarioId { get; private set; }
    public RolId RolId { get; private set; }

    // Propiedades de navegación (para EF Core)
    public Usuario? Usuario { get; private set; }
    public Rol? Rol { get; private set; }

    // Auditoría opcional (recomendada para tablas intermedias)
    public DateTime AsignadoEn { get; private set; }
    public string? AsignadoPor { get; private set; }
    public DateTime? EliminadoEn { get; private set; }

    // Constructor privado para EF Core
    private UsuarioRol()
    {
        UsuarioId = null!;
        RolId = null!;
    }

    // Constructor privado para uso interno
    private UsuarioRol(UsuarioId usuarioId, RolId rolId)
    {
        UsuarioId = usuarioId ?? throw new ArgumentNullException(nameof(usuarioId));
        RolId = rolId ?? throw new ArgumentNullException(nameof(rolId));

        AsignadoEn = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory method para crear una nueva asignación Usuario-Rol
    /// </summary>
    public static UsuarioRol Crear(UsuarioId usuarioId, RolId rolId, string? asignadoPor = null)
    {
        if (usuarioId == null) throw new ArgumentNullException(nameof(usuarioId));
        if (rolId == null) throw new ArgumentNullException(nameof(rolId));

        return new UsuarioRol(usuarioId, rolId)
        {
            Id = UsuarioRolId.New(),
            AsignadoPor = asignadoPor
        };
    }

    /// <summary>
    /// Marca la asignación como eliminada (soft delete)
    /// </summary>
    public void Desasignar(string? eliminadoPor = null)
    {
        if (EliminadoEn.HasValue) return; // Ya está desasignado

        EliminadoEn = DateTime.UtcNow;
        // Opcional: registrar quién eliminó
    }

    /// <summary>
    /// Re-activa una asignación previamente desasignada
    /// </summary>
    public void Reasignar()
    {
        if (!EliminadoEn.HasValue) return; // Ya está activa

        EliminadoEn = null;
        AsignadoEn = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica si la asignación está activa
    /// </summary>
    public bool EstaActivo() => !EliminadoEn.HasValue;
}


//using BrahmanGan.Domain.Common;

//namespace BrahmanGan.Domain.Modulos.Seguridad;

//public sealed class UsuarioRol : Entity<UsuarioRolId>
//{
//    public UsuarioId UsuarioId { get; private set; }
//    public RolId RolId { get; private set; }

//    // Navegación
//    public Usuario? Usuario { get; private set; }
//    public Rol? Rol { get; private set; }

//    private UsuarioRol() { }

//    public UsuarioRol(UsuarioId usuarioId, RolId rolId)
//    {
//        UsuarioId = usuarioId;
//        RolId = rolId;
//    }
//}
