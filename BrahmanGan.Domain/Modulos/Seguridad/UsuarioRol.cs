using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.Modulos.Seguridad;

/// <summary>Tabla de unión Usuario ↔ Rol.</summary>
public sealed class UsuarioRol
{
    public UsuarioId UsuarioId { get; }
    public RolId RolId { get; }
    public Rol? Rol { get; }

    private UsuarioRol() { UsuarioId = UsuarioId.New(); RolId = RolId.New(); }
    public UsuarioRol(UsuarioId usuarioId, RolId rolId) { UsuarioId = usuarioId; RolId = rolId; }
}
