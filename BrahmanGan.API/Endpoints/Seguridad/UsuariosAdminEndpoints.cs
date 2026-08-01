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
