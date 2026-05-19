using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Exceptions;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Exceptions; // BusinessRuleException
using BrahmanGan.Domain.Modulos.Seguridad;
using AppEntityNotFoundException = BrahmanGan.Application.Exceptions.EntityNotFoundException;

namespace BrahmanGan.Application.UseCases.Auth;

// ══════════════════════════════════════════════════════════════
//  AuthServicio — Login, Registro, Refresh, OAuth2, Logout
// ══════════════════════════════════════════════════════════════
public sealed class AuthServicio : IAuthServicio
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IRolRepository _roles;
    private readonly IJwtServicio _jwt;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;

    public AuthServicio(IUsuarioRepository usuarios, IRolRepository roles,
                        IJwtServicio jwt, IUnitOfWork uow, IPasswordHasher hasher)
    {
        _usuarios = usuarios;
        _roles = roles;
        _jwt = jwt;
        _uow = uow;
        _hasher = hasher;
    }

    // ─── Login local ─────────────────────────────────────────
    public async Task<TokenResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerConRolesPorEmailAsync(request.Email, ct)
            ?? throw new AppEntityNotFoundException("Usuario", request.Email);

        if (!usuario.Activo)
            throw new BusinessRuleException("La cuenta está desactivada.");

        if (usuario.PasswordHash is null ||
            !_hasher.Verificar(request.Password, usuario.PasswordHash))
            throw new BusinessRuleException("Credenciales incorrectas.");

        return await EmitirTokensAsync(usuario, ct);
    }

    // ─── Registro ────────────────────────────────────────────
    public async Task<TokenResponse> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default)
    {
        if (request.Password != request.ConfirmarPassword)
            throw new BusinessRuleException("Las contraseñas no coinciden.");

        if (await _usuarios.ExisteEmailAsync(request.Email, ct))
            throw new BusinessRuleException("Ya existe una cuenta con ese email.");

        var hash = _hasher.Hashear(request.Password);
        var usuario = Usuario.CrearLocal(request.Email, request.NombreCompleto, hash);

        var rolOperador = await _roles.ObtenerPorNombreAsync("Operador", ct);
        if (rolOperador is not null) usuario.AsignarRol(rolOperador);

        await _usuarios.AgregarAsync(usuario, ct);
        await _uow.SaveChangesAsync(ct);

        // Recargar con roles para el token
        var usuarioConRoles = await _usuarios.ObtenerConRolesAsync(usuario.Id, ct)
            ?? usuario;
        return await EmitirTokensAsync(usuarioConRoles, ct);
    }

    // ─── Refresh token ────────────────────────────────────────
    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var info = _jwt.ValidarAccessToken(request.AccessToken)
            ?? throw new BusinessRuleException("Access token inválido.");

        var usuario = await _usuarios.ObtenerConRolesAsync(Domain.Common.UsuarioId.From(info.Id), ct)
            ?? throw new AppEntityNotFoundException("Usuario", info.Id);

        if (usuario.RefreshToken != request.RefreshToken ||
            usuario.RefreshTokenExpira < DateTime.UtcNow)
            throw new BusinessRuleException("Refresh token inválido o expirado.");

        return await EmitirTokensAsync(usuario, ct);
    }

    // ─── OAuth2 ───────────────────────────────────────────────
    public async Task<TokenResponse> LoginOAuthAsync(string email, string nombreCompleto,
                                                      ProveedorAuth proveedor, string idExterno,
                                                      CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerPorIdExternoAsync(idExterno, proveedor, ct);

        if (usuario is null)
        {
            if (await _usuarios.ExisteEmailAsync(email, ct))
                throw new BusinessRuleException("Ya existe una cuenta local con ese email. Inicia sesión con contraseña.");

            var nuevo = Usuario.CrearOAuth(email, nombreCompleto, proveedor, idExterno);
            var rolOperador = await _roles.ObtenerPorNombreAsync("Operador", ct);
            if (rolOperador is not null) nuevo.AsignarRol(rolOperador);
            await _usuarios.AgregarAsync(nuevo, ct);
            await _uow.SaveChangesAsync(ct);
            usuario = nuevo;
        }
        else if (!usuario.Activo)
        {
            throw new BusinessRuleException("La cuenta está desactivada.");
        }

        var usuarioConRoles = await _usuarios.ObtenerConRolesAsync(usuario.Id, ct)
            ?? usuario;
        return await EmitirTokensAsync(usuarioConRoles, ct);
    }

    // ─── Logout ───────────────────────────────────────────────
    public async Task LogoutAsync(int usuarioId, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(Domain.Common.UsuarioId.From(usuarioId), ct)
            ?? throw new AppEntityNotFoundException("Usuario", usuarioId);
        usuario.RevocarRefreshToken();
        await _usuarios.ActualizarAsync(usuario, ct);
        await _uow.SaveChangesAsync(ct);
    }

    // ─── Cambiar contraseña ───────────────────────────────────
    public async Task CambiarPasswordAsync(int usuarioId, CambiarPasswordRequest request, CancellationToken ct = default)
    {
        if (request.NuevoPassword != request.ConfirmarNuevoPassword)
            throw new BusinessRuleException("Las contraseñas no coinciden.");

        var usuario = await _usuarios.ObtenerPorIdAsync(Domain.Common.UsuarioId.From(usuarioId), ct)
            ?? throw new AppEntityNotFoundException("Usuario", usuarioId);

        if (!_hasher.Verificar(request.PasswordActual, usuario.PasswordHash ?? ""))
            throw new BusinessRuleException("La contraseña actual es incorrecta.");

        usuario.CambiarPassword(_hasher.Hashear(request.NuevoPassword));
        await _usuarios.ActualizarAsync(usuario, ct);
        await _uow.SaveChangesAsync(ct);
    }

    // ─── Emitir tokens ────────────────────────────────────────
    private async Task<TokenResponse> EmitirTokensAsync(Usuario usuario, CancellationToken ct)
    {
        var roles = usuario.UsuariosRol
            .Select(ur => ur.Rol?.Nombre ?? "")
            .Where(n => !string.IsNullOrEmpty(n))
            .ToArray();

        var permisos = usuario.UsuariosRol
            .SelectMany(ur => ur.Rol?.RolesPermiso ?? (IEnumerable<RolPermiso>)[])
            .Select(rp => rp.Permiso?.Clave ?? "")
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct()
            .ToArray();

        var info = new UsuarioInfoResponse(
            usuario.Id.Value,
            usuario.Email,
            usuario.NombreCompleto,
            roles,
            permisos);

        var accessToken  = _jwt.GenerarAccessToken(info);
        var refreshToken = _jwt.GenerarRefreshToken();

        usuario.ActualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        usuario.RegistrarAcceso();
        await _usuarios.ActualizarAsync(usuario, ct);
        await _uow.SaveChangesAsync(ct);

        return new TokenResponse(accessToken, refreshToken, DateTime.UtcNow.AddMinutes(60), info);
    }
}

// ══════════════════════════════════════════════════════════════
//  RolServicio
// ══════════════════════════════════════════════════════════════
public sealed class RolServicio : IRolServicio
{
    private readonly IRolRepository _roles;
    private readonly IPermisoRepository _permisos;
    private readonly IUnitOfWork _uow;

    public RolServicio(IRolRepository roles, IPermisoRepository permisos, IUnitOfWork uow)
    {
        _roles = roles; _permisos = permisos; _uow = uow;
    }

    public async Task<IEnumerable<RolResponse>> ListarRolesAsync(CancellationToken ct = default)
        => (await _roles.ListarAsync(ct)).Select(MapRol);

    public async Task<RolResponse> ObtenerRolAsync(int id, CancellationToken ct = default)
    {
        var rol = await _roles.ObtenerConPermisosAsync(Domain.Common.RolId.From(id), ct)
            ?? throw new AppEntityNotFoundException("Rol", id);
        return MapRol(rol);
    }

    public async Task<RolResponse> CrearRolAsync(CrearRolRequest request, CancellationToken ct = default)
    {
        var rol = Rol.Crear(request.Nombre, request.Descripcion);
        await _roles.AgregarAsync(rol, ct);
        await _uow.SaveChangesAsync(ct);
        return MapRol(rol);
    }

    public async Task AsignarPermisoAsync(AsignarPermisoRolRequest request, CancellationToken ct = default)
    {
        var rol = await _roles.ObtenerConPermisosAsync(Domain.Common.RolId.From(request.RolId), ct)
            ?? throw new AppEntityNotFoundException("Rol", request.RolId);
        var permiso = await _permisos.ObtenerPorIdAsync(Domain.Common.PermisoId.From(request.PermisoId), ct)
            ?? throw new AppEntityNotFoundException("Permiso", request.PermisoId);
        rol.AsignarPermiso(permiso);
        await _roles.ActualizarAsync(rol, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task RevocarPermisoAsync(AsignarPermisoRolRequest request, CancellationToken ct = default)
    {
        var rol = await _roles.ObtenerConPermisosAsync(Domain.Common.RolId.From(request.RolId), ct)
            ?? throw new AppEntityNotFoundException("Rol", request.RolId);
        rol.RevocarPermiso(Domain.Common.PermisoId.From(request.PermisoId));
        await _roles.ActualizarAsync(rol, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private static RolResponse MapRol(Rol r) => new(
        r.Id.Value, r.Nombre, r.Descripcion, r.EsSistema, r.Activo,
        r.RolesPermiso.Select(rp => rp.Permiso is { } p
            ? new PermisoResponse(p.Id.Value, p.Modulo.ToString(), p.Accion.ToString(), p.Clave, p.Descripcion)
            : new PermisoResponse(rp.PermisoId.Value, "", "", "", "")).ToArray());
}

// ══════════════════════════════════════════════════════════════
//  UsuarioAdminServicio
// ══════════════════════════════════════════════════════════════
public sealed class UsuarioAdminServicio : IUsuarioAdminServicio
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IRolRepository _roles;
    private readonly IUnitOfWork _uow;

    public UsuarioAdminServicio(IUsuarioRepository usuarios, IRolRepository roles, IUnitOfWork uow)
    {
        _usuarios = usuarios; _roles = roles; _uow = uow;
    }

    public async Task<IEnumerable<UsuarioResponse>> ListarUsuariosAsync(CancellationToken ct = default)
        => (await _usuarios.ListarAsync(ct)).Select(MapUsuario);

    public async Task<UsuarioResponse> ObtenerUsuarioAsync(int id, CancellationToken ct = default)
    {
        var u = await _usuarios.ObtenerConRolesAsync(Domain.Common.UsuarioId.From(id), ct)
            ?? throw new AppEntityNotFoundException("Usuario", id);
        return MapUsuario(u);
    }

    public async Task AsignarRolAsync(AsignarRolUsuarioRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerConRolesAsync(Domain.Common.UsuarioId.From(request.UsuarioId), ct)
            ?? throw new AppEntityNotFoundException("Usuario", request.UsuarioId);
        var rol = await _roles.ObtenerPorIdAsync(Domain.Common.RolId.From(request.RolId), ct)
            ?? throw new AppEntityNotFoundException("Rol", request.RolId);
        usuario.AsignarRol(rol);
        await _usuarios.ActualizarAsync(usuario, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task RevocarRolAsync(AsignarRolUsuarioRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerConRolesAsync(Domain.Common.UsuarioId.From(request.UsuarioId), ct)
            ?? throw new AppEntityNotFoundException("Usuario", request.UsuarioId);
        usuario.RevocarRol(Domain.Common.RolId.From(request.RolId));
        await _usuarios.ActualizarAsync(usuario, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task DesactivarUsuarioAsync(int id, CancellationToken ct = default)
    {
        var u = await _usuarios.ObtenerPorIdAsync(Domain.Common.UsuarioId.From(id), ct)
            ?? throw new AppEntityNotFoundException("Usuario", id);
        u.Desactivar();
        await _usuarios.ActualizarAsync(u, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task ActivarUsuarioAsync(int id, CancellationToken ct = default)
    {
        var u = await _usuarios.ObtenerPorIdAsync(Domain.Common.UsuarioId.From(id), ct)
            ?? throw new AppEntityNotFoundException("Usuario", id);
        u.Activar();
        await _usuarios.ActualizarAsync(u, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private static UsuarioResponse MapUsuario(Usuario u) => new(
        u.Id.Value, u.Email, u.NombreCompleto, u.Proveedor.ToString(),
        u.Activo, u.FechaCreacion, u.UltimoAcceso,
        u.UsuariosRol.Select(ur => ur.Rol?.Nombre ?? "").Where(n => n != "").ToArray());
}
