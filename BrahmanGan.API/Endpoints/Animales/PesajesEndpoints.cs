using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Animales;

public sealed class RegistrarPesajeEndpoint(IPesajeService svc) : Endpoint<RegistrarPesajeRequest, PesajeResponse>
{
    public override void Configure()
    {
        Post("api/pesajes");
    }

    public override async Task HandleAsync(RegistrarPesajeRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarAsync(req, ct), ct);
}

public sealed class ListarPesajesPorAnimalEndpoint(IPesajeService svc) : EndpointWithoutRequest<IReadOnlyList<PesajeResponse>>
{
    public override void Configure()
    {
        Get("api/pesajes/animal/{idAnimal:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorAnimalAsync(Route<int>("idAnimal"), ct), ct);
}
