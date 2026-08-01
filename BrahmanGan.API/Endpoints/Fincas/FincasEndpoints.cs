using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Fincas;

public sealed class CrearFincaEndpoint(IFincaService svc) : Endpoint<CrearFincaRequest, FincaResponse>
{
    public override void Configure()
    {
        Post("api/fincas");
    }

    public override async Task HandleAsync(CrearFincaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ObtenerFincaEndpoint(IFincaService svc) : EndpointWithoutRequest<FincaResponse>
{
    public override void Configure()
    {
        Get("api/fincas/{id:int}");
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

public sealed class ListarFincasEndpoint(IFincaService svc) : EndpointWithoutRequest<IReadOnlyList<FincaResponse>>
{
    public override void Configure()
    {
        Get("api/fincas");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}
