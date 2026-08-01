using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Pastoreo;

public sealed class CrearPlanPastoreoEndpoint(IPastoreoService svc) : Endpoint<CrearPlanPastoreoRequest, PlanPastoreoResponse>
{
    public override void Configure()
    {
        Post("api/pastoreo/planes");
        Permissions("Finca:Crear");
    }

    public override async Task HandleAsync(CrearPlanPastoreoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearPlanAsync(req, ct), ct);
}

public sealed class ObtenerPlanPastoreoEndpoint(IPastoreoService svc) : EndpointWithoutRequest<PlanPastoreoResponse>
{
    public override void Configure()
    {
        Get("api/pastoreo/planes/{id:int}");
        Permissions("Finca:Leer");
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

public sealed class ListarPlanesPastoreoEndpoint(IPastoreoService svc) : EndpointWithoutRequest<IReadOnlyList<PlanPastoreoResponse>>
{
    public override void Configure()
    {
        Get("api/pastoreo/planes");
        Permissions("Finca:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPlanesAsync(ct), ct);
}

public sealed class ListarPlanesPastoreoPorPotreroEndpoint(IPastoreoService svc) : EndpointWithoutRequest<IReadOnlyList<PlanPastoreoResponse>>
{
    public override void Configure()
    {
        Get("api/pastoreo/planes/potrero/{idPotrero:int}");
        Permissions("Finca:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorPotreroAsync(Route<int>("idPotrero"), ct), ct);
}

public sealed class FinalizarPlanPastoreoEndpoint(IPastoreoService svc) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Patch("api/pastoreo/planes/{id:int}/finalizar");
        Permissions("Finca:Editar");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await svc.FinalizarPlanAsync(Route<int>("id"), Query<DateOnly>("fechaFin"), ct);
        await Send.NoContentAsync(ct);
    }
}
