using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Costos;

public sealed class CrearIngresoEndpoint(IIngresoService svc) : Endpoint<CrearIngresoRequest, IngresoResponse>
{
    public override void Configure()
    {
        Post("api/ingresos");
        Permissions("Costos:Crear");
    }

    public override async Task HandleAsync(CrearIngresoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ListarIngresosPorCentroEndpoint(IIngresoService svc) : EndpointWithoutRequest<IReadOnlyList<IngresoResponse>>
{
    public override void Configure()
    {
        Get("api/ingresos/centro/{idCentro:int}");
        Permissions("Costos:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorCentroAsync(Route<int>("idCentro"), ct), ct);
}
