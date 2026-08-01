using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Alimentacion;

public sealed class CrearPlanAlimentacionEndpoint(IAlimentacionService svc) : Endpoint<CrearPlanAlimentacionRequest, PlanAlimentacionResponse>
{
    public override void Configure()
    {
        Post("api/alimentacion/planes");
        Permissions("Inventario:Crear");
    }

    public override async Task HandleAsync(CrearPlanAlimentacionRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearPlanAsync(req, ct), ct);
}

public sealed class ObtenerPlanAlimentacionEndpoint(IAlimentacionService svc) : EndpointWithoutRequest<PlanAlimentacionResponse>
{
    public override void Configure()
    {
        Get("api/alimentacion/planes/{id:int}");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var r = await svc.ObtenerPlanAsync(Route<int>("id"), ct);
        if (r is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync(r, ct);
    }
}

public sealed class ListarPlanesAlimentacionEndpoint(IAlimentacionService svc) : EndpointWithoutRequest<IReadOnlyList<PlanAlimentacionResponse>>
{
    public override void Configure()
    {
        Get("api/alimentacion/planes");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPlanesAsync(ct), ct);
}

public sealed class ListarPlanesAlimentacionPorFincaEndpoint(IAlimentacionService svc) : EndpointWithoutRequest<IReadOnlyList<PlanAlimentacionResponse>>
{
    public override void Configure()
    {
        Get("api/alimentacion/planes/finca/{idFinca:int}");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorFincaAsync(Route<int>("idFinca"), ct), ct);
}

public sealed class DesactivarPlanAlimentacionEndpoint(IAlimentacionService svc) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Patch("api/alimentacion/planes/{id:int}/desactivar");
        Permissions("Inventario:Editar");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await svc.DesactivarPlanAsync(Route<int>("id"), ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class AgregarDetallePlanEndpoint(IAlimentacionService svc) : Endpoint<AgregarDetallePlanRequest, DetallePlanAlimentacionResponse>
{
    public override void Configure()
    {
        Post("api/alimentacion/detalles");
        Permissions("Inventario:Crear");
    }

    public override async Task HandleAsync(AgregarDetallePlanRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.AgregarDetalleAsync(req, ct), ct);
}

public sealed class ListarDetallesPlanEndpoint(IAlimentacionService svc) : EndpointWithoutRequest<IReadOnlyList<DetallePlanAlimentacionResponse>>
{
    public override void Configure()
    {
        Get("api/alimentacion/planes/{idPlan:int}/detalles");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarDetallesAsync(Route<int>("idPlan"), ct), ct);
}
