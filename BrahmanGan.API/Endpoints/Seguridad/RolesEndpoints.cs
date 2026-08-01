using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Seguridad;

/// <summary>Administración de roles y permisos del sistema. Requiere rol Administrador.</summary>
public sealed class ListarRolesEndpoint(IRolServicio roles) : EndpointWithoutRequest<IEnumerable<RolResponse>>
{
    public override void Configure()
    {
        Get("api/roles");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await roles.ListarRolesAsync(ct), ct);
}

public sealed class ObtenerRolEndpoint(IRolServicio roles) : EndpointWithoutRequest<RolResponse>
{
    public override void Configure()
    {
        Get("api/roles/{id:int}");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await roles.ObtenerRolAsync(Route<int>("id"), ct), ct);
}

public sealed class CrearRolEndpoint(IRolServicio roles) : Endpoint<CrearRolRequest, RolResponse>
{
    public override void Configure()
    {
        Post("api/roles");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CrearRolRequest req, CancellationToken ct)
    {
        var rol = await roles.CrearRolAsync(req, ct);
        await Send.CreatedAtAsync<ObtenerRolEndpoint>(new { id = rol.Id }, rol, cancellation: ct);
    }
}

public sealed class AsignarPermisoRolEndpoint(IRolServicio roles) : Endpoint<AsignarPermisoRolRequest>
{
    public override void Configure()
    {
        Post("api/roles/permisos");
        Policies("Administrador");
    }

    public override async Task HandleAsync(AsignarPermisoRolRequest req, CancellationToken ct)
    {
        await roles.AsignarPermisoAsync(req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class RevocarPermisoRolEndpoint(IRolServicio roles) : Endpoint<AsignarPermisoRolRequest>
{
    public override void Configure()
    {
        Delete("api/roles/permisos");
        Policies("Administrador");
    }

    public override async Task HandleAsync(AsignarPermisoRolRequest req, CancellationToken ct)
    {
        await roles.RevocarPermisoAsync(req, ct);
        await Send.NoContentAsync(ct);
    }
}
