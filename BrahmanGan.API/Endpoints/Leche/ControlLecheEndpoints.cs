using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Leche;

public sealed class RegistrarControlLecheEndpoint(IControlLecheService svc) : Endpoint<RegistrarControlLecheRequest, ControlLecheResponse>
{
    public override void Configure()
    {
        Post("api/control-leche");
    }

    public override async Task HandleAsync(RegistrarControlLecheRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarAsync(req, ct), ct);
}

public sealed class ListarControlLechePorAnimalEndpoint(IControlLecheService svc) : EndpointWithoutRequest<IReadOnlyList<ControlLecheResponse>>
{
    public override void Configure()
    {
        Get("api/control-leche/animal/{idAnimal:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(
            await svc.ListarPorAnimalAsync(
                Route<int>("idAnimal"),
                Query<DateOnly>("desde"),
                Query<DateOnly>("hasta"),
                ct),
            ct);
}

// ── Parámetros Lactancia ───────────────────────────────────────
public sealed class IniciarLactanciaEndpoint(IControlLecheService svc) : Endpoint<IniciarParametroLactanciaRequest, ParametroLactanciaResponse>
{
    public override void Configure()
    {
        Post("api/control-leche/lactancias");
    }

    public override async Task HandleAsync(IniciarParametroLactanciaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.IniciarLactanciaAsync(req, ct), ct);
}

public sealed class CerrarLactanciaEndpoint(IControlLecheService svc) : Endpoint<CerrarParametroLactanciaRequest, ParametroLactanciaResponse>
{
    public override void Configure()
    {
        Patch("api/control-leche/lactancias/{id:int}/cerrar");
    }

    public override async Task HandleAsync(CerrarParametroLactanciaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CerrarLactanciaAsync(Route<int>("id"), req, ct), ct);
}

public sealed class ListarLactanciasPorAnimalEndpoint(IControlLecheService svc) : EndpointWithoutRequest<IReadOnlyList<ParametroLactanciaResponse>>
{
    public override void Configure()
    {
        Get("api/control-leche/lactancias/animal/{idAnimal:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarLactanciasPorAnimalAsync(Route<int>("idAnimal"), ct), ct);
}

// ── Calidad Leche ──────────────────────────────────────────────
public sealed class RegistrarCalidadLecheEndpoint(IControlLecheService svc) : Endpoint<RegistrarCalidadLecheRequest, CalidadLecheResponse>
{
    public override void Configure()
    {
        Post("api/control-leche/calidad");
    }

    public override async Task HandleAsync(RegistrarCalidadLecheRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarCalidadAsync(req, ct), ct);
}

public sealed class ListarCalidadLechePorFechaEndpoint(IControlLecheService svc) : EndpointWithoutRequest<IReadOnlyList<CalidadLecheResponse>>
{
    public override void Configure()
    {
        Get("api/control-leche/calidad");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(
            await svc.ListarCalidadPorFechaAsync(
                Query<DateOnly>("desde"),
                Query<DateOnly>("hasta"),
                ct),
            ct);
}
