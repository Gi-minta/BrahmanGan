using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Costos;

public sealed class CrearCentroCostoEndpoint(ICentroCostoService svc) : Endpoint<CrearCentroCostoRequest, CentroCostoResponse>
{
    public override void Configure()
    {
        Post("api/centros-costo");
        Permissions("Costos:Crear");
    }

    public override async Task HandleAsync(CrearCentroCostoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ListarCentrosCostoEndpoint(ICentroCostoService svc) : EndpointWithoutRequest<IReadOnlyList<CentroCostoResponse>>
{
    public override void Configure()
    {
        Get("api/centros-costo");
        Permissions("Costos:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}
