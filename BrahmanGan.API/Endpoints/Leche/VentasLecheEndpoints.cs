using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Leche;

public sealed class RegistrarVentaLecheEndpoint(IVentaLecheService svc) : Endpoint<RegistrarVentaLecheRequest, VentaLecheResponse>
{
    public override void Configure()
    {
        Post("api/ventas-leche");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrarVentaLecheRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarAsync(req, ct), ct);
}
