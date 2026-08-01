using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Reproduccion;

public sealed class RegistrarMontaEndpoint(IServicioReproductivoService svc) : Endpoint<RegistrarMontaRequest, ServicioResponse>
{
    public override void Configure()
    {
        Post("api/servicios-reproductivos/monta");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrarMontaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarMontaAsync(req, ct), ct);
}

public sealed class RegistrarIaEndpoint(IServicioReproductivoService svc) : Endpoint<RegistrarIaRequest, ServicioResponse>
{
    public override void Configure()
    {
        Post("api/servicios-reproductivos/ia");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrarIaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarIaAsync(req, ct), ct);
}

public sealed class ConfirmarServicioEndpoint(IServicioReproductivoService svc) : Endpoint<ConfirmarServicioRequest>
{
    public override void Configure()
    {
        Put("api/servicios-reproductivos/{id:int}/confirmar");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConfirmarServicioRequest req, CancellationToken ct)
    {
        await svc.ConfirmarAsync(Route<int>("id"), req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class ListarServiciosPorHembraEndpoint(IServicioReproductivoService svc) : EndpointWithoutRequest<IReadOnlyList<ServicioResponse>>
{
    public override void Configure()
    {
        Get("api/servicios-reproductivos/hembra/{idHembra:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorHembraAsync(Route<int>("idHembra"), ct), ct);
}

// ── Semen ──────────────────────────────────────────────────────
public sealed class CrearSemenEndpoint(IServicioReproductivoService svc) : Endpoint<CrearSemenRequest, SemenResponse>
{
    public override void Configure()
    {
        Post("api/servicios-reproductivos/semen");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CrearSemenRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearSemenAsync(req, ct), ct);
}

public sealed class ListarSemenEndpoint(IServicioReproductivoService svc) : EndpointWithoutRequest<IReadOnlyList<SemenResponse>>
{
    public override void Configure()
    {
        Get("api/servicios-reproductivos/semen");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarSemenAsync(ct), ct);
}

public sealed class ObtenerSemenEndpoint(IServicioReproductivoService svc) : EndpointWithoutRequest<SemenResponse>
{
    public override void Configure()
    {
        Get("api/servicios-reproductivos/semen/{id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var r = await svc.ObtenerSemenAsync(Route<int>("id"), ct);
        if (r is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync(r, ct);
    }
}

public sealed class AjustarStockSemenEndpoint(IServicioReproductivoService svc) : Endpoint<AjustarStockSemenRequest, SemenResponse>
{
    public override void Configure()
    {
        Patch("api/servicios-reproductivos/semen/stock");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AjustarStockSemenRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.AjustarStockSemenAsync(req, ct), ct);
}

// ── Nacimientos ────────────────────────────────────────────────
public sealed class ObtenerNacimientoEndpoint(IServicioReproductivoService svc) : EndpointWithoutRequest<NacimientoResponse>
{
    public override void Configure()
    {
        Get("api/servicios-reproductivos/nacimientos/{id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var r = await svc.ObtenerNacimientoAsync(Route<int>("id"), ct);
        if (r is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync(r, ct);
    }
}

public sealed class ListarNacimientosPorGestacionEndpoint(IServicioReproductivoService svc) : EndpointWithoutRequest<IReadOnlyList<NacimientoResponse>>
{
    public override void Configure()
    {
        Get("api/servicios-reproductivos/nacimientos/gestacion/{idGestacion:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarNacimientosPorGestacionAsync(Route<int>("idGestacion"), ct), ct);
}
