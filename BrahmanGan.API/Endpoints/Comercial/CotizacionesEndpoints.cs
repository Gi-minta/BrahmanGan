using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Comercial;

public sealed class CrearCotizacionEndpoint(ICotizacionVentaService svc) : Endpoint<CrearCotizacionRequest, CotizacionResponse>
{
    public override void Configure()
    {
        Post("api/cotizaciones");
        Permissions("Comercial:Crear");
    }

    public override async Task HandleAsync(CrearCotizacionRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ObtenerCotizacionEndpoint(ICotizacionVentaService svc) : EndpointWithoutRequest<CotizacionResponse>
{
    public override void Configure()
    {
        Get("api/cotizaciones/{id:int}");
        Permissions("Comercial:Leer");
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

public sealed class AgregarDetalleCotizacionEndpoint(ICotizacionVentaService svc) : Endpoint<AgregarDetalleCotizacionRequest>
{
    public override void Configure()
    {
        Post("api/cotizaciones/{id:int}/detalle");
        Permissions("Comercial:Crear");
    }

    public override async Task HandleAsync(AgregarDetalleCotizacionRequest req, CancellationToken ct)
    {
        await svc.AgregarDetalleAsync(Route<int>("id"), req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class AprobarCotizacionEndpoint(ICotizacionVentaService svc) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Put("api/cotizaciones/{id:int}/aprobar");
        Permissions("Comercial:Editar");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await svc.AprobarAsync(Route<int>("id"), ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class RechazarCotizacionEndpoint(ICotizacionVentaService svc) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Put("api/cotizaciones/{id:int}/rechazar");
        Permissions("Comercial:Editar");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await svc.RechazarAsync(Route<int>("id"), ct);
        await Send.NoContentAsync(ct);
    }
}
