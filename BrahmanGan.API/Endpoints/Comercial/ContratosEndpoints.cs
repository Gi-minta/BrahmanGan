using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Comercial;

public sealed class CrearContratoEndpoint(IContratoService svc) : Endpoint<CrearContratoRequest, ContratoResponse>
{
    public override void Configure()
    {
        Post("api/contratos");
    }

    public override async Task HandleAsync(CrearContratoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ListarContratosPorClienteEndpoint(IContratoService svc) : EndpointWithoutRequest<IReadOnlyList<ContratoResponse>>
{
    public override void Configure()
    {
        Get("api/contratos/cliente/{idCliente:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorClienteAsync(Route<int>("idCliente"), ct), ct);
}
