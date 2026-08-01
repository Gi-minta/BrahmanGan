using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Equipos;

public sealed class CrearMaquinariaEndpoint(IMaquinariaService svc) : Endpoint<CrearMaquinariaRequest, MaquinariaResponse>
{
    public override void Configure()
    {
        Post("api/maquinaria");
        Permissions("Equipos:Crear");
    }

    public override async Task HandleAsync(CrearMaquinariaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ObtenerMaquinariaEndpoint(IMaquinariaService svc) : EndpointWithoutRequest<MaquinariaResponse>
{
    public override void Configure()
    {
        Get("api/maquinaria/{id:int}");
        Permissions("Equipos:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var r = await svc.ObtenerAsync(Route<int>("id"), ct);
        if (r is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync(r, ct);
    }
}

public sealed class ListarMaquinariaEndpoint(IMaquinariaService svc) : EndpointWithoutRequest<IReadOnlyList<MaquinariaResponse>>
{
    public override void Configure()
    {
        Get("api/maquinaria");
        Permissions("Equipos:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}

public sealed class RegistrarMantenimientoEndpoint(IMaquinariaService svc) : Endpoint<RegistrarMantenimientoRequest, MantenimientoEquipoResponse>
{
    public override void Configure()
    {
        Post("api/maquinaria/mantenimiento");
        Permissions("Equipos:Crear");
    }

    public override async Task HandleAsync(RegistrarMantenimientoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarMantenimientoAsync(req, ct), ct);
}

public sealed class ListarMantenimientosEndpoint(IMaquinariaService svc) : EndpointWithoutRequest<IReadOnlyList<MantenimientoEquipoResponse>>
{
    public override void Configure()
    {
        Get("api/maquinaria/{id:int}/mantenimientos");
        Permissions("Equipos:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarMantenimientosAsync(Route<int>("id"), ct), ct);
}
