using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Leche;

public sealed class RegistrarProduccionLecheEndpoint(IProduccionLecheService svc) : Endpoint<RegistrarProduccionLecheRequest, ProduccionLecheResponse>
{
    public override void Configure()
    {
        Post("api/produccion-leche");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrarProduccionLecheRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarAsync(req, ct), ct);
}
