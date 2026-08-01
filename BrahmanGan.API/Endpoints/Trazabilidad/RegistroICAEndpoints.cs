using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Trazabilidad;

public sealed class EmitirRegistroICAEndpoint(IRegistroICAService svc) : Endpoint<EmitirRegistroICARequest, RegistroICAResponse>
{
    public override void Configure()
    {
        Post("api/registros-ica");
    }

    public override async Task HandleAsync(EmitirRegistroICARequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.EmitirAsync(req, ct), ct);
}

public sealed class ListarRegistrosICAPorAnimalEndpoint(IRegistroICAService svc) : EndpointWithoutRequest<IReadOnlyList<RegistroICAResponse>>
{
    public override void Configure()
    {
        Get("api/registros-ica/animal/{idAnimal:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorAnimalAsync(Route<int>("idAnimal"), ct), ct);
}

public sealed class RegistrosICAProximosVencerEndpoint(IRegistroICAService svc) : EndpointWithoutRequest<IReadOnlyList<RegistroICAResponse>>
{
    public override void Configure()
    {
        Get("api/registros-ica/proximos-vencer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarProximosVencerAsync(Query<int?>("dias", isRequired: false) ?? 30, ct), ct);
}

public sealed class CancelarRegistroICAEndpoint(IRegistroICAService svc) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("api/registros-ica/{id:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await svc.CancelarAsync(Route<int>("id"), ct);
        await Send.NoContentAsync(ct);
    }
}
