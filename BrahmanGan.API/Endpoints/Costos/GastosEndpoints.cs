using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Costos;

public sealed class CrearGastoGeneralEndpoint(IGastoGeneralService svc) : Endpoint<CrearGastoGeneralRequest, GastoGeneralResponse>
{
    public override void Configure()
    {
        Post("api/gastos-generales");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CrearGastoGeneralRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ListarGastosPorPeriodoEndpoint(IGastoGeneralService svc) : EndpointWithoutRequest<IReadOnlyList<GastoGeneralResponse>>
{
    public override void Configure()
    {
        Get("api/gastos-generales");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(
            await svc.ListarPorPeriodoAsync(
                Query<DateOnly>("desde"),
                Query<DateOnly>("hasta"),
                ct),
            ct);
}
