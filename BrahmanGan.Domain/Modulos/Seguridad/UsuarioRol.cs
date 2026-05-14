using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.Modulos.Seguridad;

public sealed class UsuarioRol : Entity<UsuarioRolId>
{
    public UsuarioId UsuarioId { get; private set; }
    public RolId RolId { get; private set; }

    // Navegación
    public Usuario? Usuario { get; private set; }
    public Rol? Rol { get; private set; }

    private UsuarioRol() { }

    public UsuarioRol(UsuarioId usuarioId, RolId rolId)
    {
        UsuarioId = usuarioId;
        RolId = rolId;
    }
}
