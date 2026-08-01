using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Seguridad;

/// <summary>Administración de usuarios del sistema. Requiere rol Administrador.</summary>
public sealed class ListarUsuariosEndpoint(IUsuarioAdminServicio admin) : EndpointWithoutRequest<IEnumerable<UsuarioResponse>>
{
    public override void Configure()
    {
        Get("api/usuarios");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await admin.ListarUsuariosAsync(ct), ct);
}

/// <summary>
/// Alta de usuario. Es la única vía: el registro público se retiró porque permitía a
/// cualquiera crearse una cuenta en la API.
/// </summary>
public sealed class CrearUsuarioAdminEndpoint(IUsuarioAdminServicio admin)
    : Endpoint<CrearUsuarioAdminRequest, UsuarioResponse>
{
    public override void Configure()
    {
        Post("api/usuarios");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CrearUsuarioAdminRequest req, CancellationToken ct)
        => await Send.ResponseAsync(await admin.CrearUsuarioAsync(req, ct), 201, ct);
}

/// <summary>
/// Devuelve el acceso a un usuario fijándole una contraseña temporal, que tendrá que
/// cambiar al entrar. Sustituye a un flujo de recuperación por correo, que exigiría un
/// proveedor de email.
/// </summary>
public sealed class RestablecerPasswordUsuarioEndpoint(IUsuarioAdminServicio admin)
    : Endpoint<RestablecerPasswordAdminRequest>
{
    public override void Configure()
    {
        Post("api/usuarios/{id:int}/restablecer-password");
        Policies("Administrador");
    }

    public override async Task HandleAsync(RestablecerPasswordAdminRequest req, CancellationToken ct)
    {
        await admin.RestablecerPasswordAsync(Route<int>("id"), req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class ObtenerUsuarioEndpoint(IUsuarioAdminServicio admin) : EndpointWithoutRequest<UsuarioResponse>
{
    public override void Configure()
    {
        Get("api/usuarios/{id:int}");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await admin.ObtenerUsuarioAsync(Route<int>("id"), ct), ct);
}

public sealed class AsignarRolUsuarioEndpoint(IUsuarioAdminServicio admin) : Endpoint<AsignarRolUsuarioRequest>
{
    public override void Configure()
    {
        Post("api/usuarios/roles");
        Policies("Administrador");
    }

    public override async Task HandleAsync(AsignarRolUsuarioRequest req, CancellationToken ct)
    {
        await admin.AsignarRolAsync(req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class RevocarRolUsuarioEndpoint(IUsuarioAdminServicio admin) : Endpoint<AsignarRolUsuarioRequest>
{
    public override void Configure()
    {
        Delete("api/usuarios/roles");
        Policies("Administrador");
    }

    public override async Task HandleAsync(AsignarRolUsuarioRequest req, CancellationToken ct)
    {
        await admin.RevocarRolAsync(req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class DesactivarUsuarioEndpoint(IUsuarioAdminServicio admin) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Patch("api/usuarios/{id:int}/desactivar");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await admin.DesactivarUsuarioAsync(Route<int>("id"), ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class ActivarUsuarioEndpoint(IUsuarioAdminServicio admin) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Patch("api/usuarios/{id:int}/activar");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await admin.ActivarUsuarioAsync(Route<int>("id"), ct);
        await Send.NoContentAsync(ct);
    }
}
