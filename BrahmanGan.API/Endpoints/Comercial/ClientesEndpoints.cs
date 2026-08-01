using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Comercial;

public sealed class CrearClienteEndpoint(IClienteService svc) : Endpoint<CrearClienteRequest, ClienteResponse>
{
    public override void Configure()
    {
        Post("api/clientes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CrearClienteRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ObtenerClienteEndpoint(IClienteService svc) : EndpointWithoutRequest<ClienteResponse>
{
    public override void Configure()
    {
        Get("api/clientes/{id:int}");
        AllowAnonymous();
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

public sealed class ListarClientesEndpoint(IClienteService svc) : EndpointWithoutRequest<IReadOnlyList<ClienteResponse>>
{
    public override void Configure()
    {
        Get("api/clientes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}
