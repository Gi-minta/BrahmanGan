using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Animales;

public sealed class CrearRazaEndpoint(IRazaService svc) : Endpoint<CrearRazaRequest, RazaResponse>
{
    public override void Configure()
    {
        Post("api/razas");
    }

    public override async Task HandleAsync(CrearRazaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ListarRazasEndpoint(IRazaService svc) : EndpointWithoutRequest<IReadOnlyList<RazaResponse>>
{
    public override void Configure()
    {
        Get("api/razas");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}
