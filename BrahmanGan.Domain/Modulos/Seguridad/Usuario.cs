using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Seguridad;

/// <summary>
/// Usuario del sistema ERP.
/// Soporta autenticación local (email/hash) y OAuth2 (Google/Microsoft).
/// </summary>
public sealed class Usuario : AggregateRoot<UsuarioId>
{
    public string Email { get; private set; } = string.Empty;
    public string NombreCompleto { get; private set; } = string.Empty;
    public string? PasswordHash { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpira { get; private set; }
    public ProveedorAuth Proveedor { get; private set; } = ProveedorAuth.Local;
    public string? IdExterno { get; private set; }
    public bool EmailConfirmado { get; private set; }
    public bool Activo { get; private set; } = true;
    public DateTime FechaCreacion { get; private set; } = DateTime.UtcNow;
    public DateTime? UltimoAcceso { get; private set; }

    private readonly List<UsuarioRol> _usuariosRol = [];
    public IReadOnlyCollection<UsuarioRol> UsuariosRol => _usuariosRol.AsReadOnly();

    private Usuario() { }

    // ──────────────────────────────────────────────
    /// <summary>Crear usuario local con email + hash de contraseña.</summary>
    public static Usuario CrearLocal(string email, string nombreCompleto, string passwordHash)
    {
        ValidarEmail(email);
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new BusinessRuleException("El hash de contraseña es requerido.");
        return new Usuario
        {
            Id = UsuarioId.New(),
            Email = email.ToLowerInvariant().Trim(),
            NombreCompleto = nombreCompleto.Trim(),
            PasswordHash = passwordHash,
            Proveedor = ProveedorAuth.Local,
            EmailConfirmado = false,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
        };
    }

    /// <summary>Crear usuario proveniente de OAuth2 (Google / Microsoft).</summary>
    public static Usuario CrearOAuth(string email, string nombreCompleto,
                                     ProveedorAuth proveedor, string idExterno)
    {
        ValidarEmail(email);
        if (string.IsNullOrWhiteSpace(idExterno))
            throw new BusinessRuleException("El identificador externo es requerido para OAuth2.");
        return new Usuario
        {
            Id = UsuarioId.New(),
            Email = email.ToLowerInvariant().Trim(),
            NombreCompleto = nombreCompleto.Trim(),
            Proveedor = proveedor,
            IdExterno = idExterno,
            EmailConfirmado = true, // OAuth ya confirma el email
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
        };
    }

    // ──────────────────────────────────────────────
    public void AsignarRol(Rol rol)
    {
        if (_usuariosRol.Any(ur => ur.RolId == rol.Id)) return;
        _usuariosRol.Add(new UsuarioRol(this.Id, rol.Id));
    }

    public void RevocarRol(RolId rolId)
    {
        var ur = _usuariosRol.FirstOrDefault(u => u.RolId == rolId);
        if (ur is not null) _usuariosRol.Remove(ur);
    }

    public void ActualizarRefreshToken(string token, DateTime expira)
    {
        RefreshToken = token;
        RefreshTokenExpira = expira;
    }

    public void RevocarRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpira = null;
    }

    public void RegistrarAcceso() => UltimoAcceso = DateTime.UtcNow;

    public void ConfirmarEmail() => EmailConfirmado = true;

    public void CambiarPassword(string nuevoHash)
    {
        if (Proveedor != ProveedorAuth.Local)
            throw new BusinessRuleException("Los usuarios OAuth2 no usan contraseña local.");
        PasswordHash = nuevoHash;
    }

    public void Desactivar()
    {
        if (!Activo) return;
        Activo = false;
    }

    public void Activar() => Activo = true;

    public void ActualizarPerfil(string nombreCompleto) =>
        NombreCompleto = nombreCompleto.Trim();

    // ──────────────────────────────────────────────
    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new BusinessRuleException("El email proporcionado no es válido.");
    }
}
