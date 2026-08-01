using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Sanidad;

public sealed class RegistrarComplementoEndpoint(IComplementoService svc) : Endpoint<RegistrarComplementoRequest, ComplementoResponse>
{
    public override void Configure()
    {
        Post("api/complementos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrarComplementoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarAsync(req, ct), ct);
}

public sealed class ListarComplementosPorTratamientoEndpoint(IComplementoService svc) : EndpointWithoutRequest<IReadOnlyList<ComplementoResponse>>
{
    public override void Configure()
    {
        Get("api/complementos/tratamiento/{idTratamiento:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorTratamientoAsync(Route<int>("idTratamiento"), ct), ct);
}
