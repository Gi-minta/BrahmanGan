using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Sostenibilidad;

// ── Captura de carbono ──────────────────────────────────────
public sealed class RegistrarCapturaCarbonoEndpoint(ISostenibilidadService svc) : Endpoint<RegistrarCapturaCarbonoRequest, CapturaCarbonoResponse>
{
    public override void Configure()
    {
        Post("api/sostenibilidad/carbono");
        Permissions("Sostenibilidad:Crear");
    }

    public override async Task HandleAsync(RegistrarCapturaCarbonoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarCapturaAsync(req, ct), ct);
}

public sealed class ListarCapturasCarbonoPorFincaEndpoint(ISostenibilidadService svc) : EndpointWithoutRequest<IReadOnlyList<CapturaCarbonoResponse>>
{
    public override void Configure()
    {
        Get("api/sostenibilidad/carbono/finca/{idFinca:int}");
        Permissions("Sostenibilidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarCapturasPorFincaAsync(Route<int>("idFinca"), ct), ct);
}

// ── Consumo de agua ─────────────────────────────────────────
public sealed class RegistrarConsumoAguaEndpoint(ISostenibilidadService svc) : Endpoint<RegistrarConsumoAguaRequest, ConsumoAguaResponse>
{
    public override void Configure()
    {
        Post("api/sostenibilidad/agua");
        Permissions("Sostenibilidad:Crear");
    }

    public override async Task HandleAsync(RegistrarConsumoAguaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarConsumoAguaAsync(req, ct), ct);
}

public sealed class ListarConsumoAguaPorFincaEndpoint(ISostenibilidadService svc) : EndpointWithoutRequest<IReadOnlyList<ConsumoAguaResponse>>
{
    public override void Configure()
    {
        Get("api/sostenibilidad/agua/finca/{idFinca:int}");
        Permissions("Sostenibilidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarConsumoAguaPorFincaAsync(Route<int>("idFinca"), ct), ct);
}

// ── Eventos medioambientales ────────────────────────────────
public sealed class RegistrarEventoMedioambientalEndpoint(ISostenibilidadService svc) : Endpoint<RegistrarEventoMedioambientalRequest, EventoMedioambientalResponse>
{
    public override void Configure()
    {
        Post("api/sostenibilidad/eventos");
        Permissions("Sostenibilidad:Crear");
    }

    public override async Task HandleAsync(RegistrarEventoMedioambientalRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarEventoAsync(req, ct), ct);
}

public sealed class ListarEventosMedioambientalesPorFincaEndpoint(ISostenibilidadService svc) : EndpointWithoutRequest<IReadOnlyList<EventoMedioambientalResponse>>
{
    public override void Configure()
    {
        Get("api/sostenibilidad/eventos/finca/{idFinca:int}");
        Permissions("Sostenibilidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarEventosPorFincaAsync(Route<int>("idFinca"), ct), ct);
}
