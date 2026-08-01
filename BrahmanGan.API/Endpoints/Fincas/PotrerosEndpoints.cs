using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Fincas;

public sealed class CrearPotreroEndpoint(IPotreroService svc) : Endpoint<CrearPotreroRequest, PotreroResponse>
{
    public override void Configure()
    {
        Post("api/potreros");
    }

    public override async Task HandleAsync(CrearPotreroRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ListarPotrerosPorFincaEndpoint(IPotreroService svc) : EndpointWithoutRequest<IReadOnlyList<PotreroResponse>>
{
    public override void Configure()
    {
        Get("api/potreros/finca/{idFinca:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorFincaAsync(Route<int>("idFinca"), ct), ct);
}

// ── Grupos de Manejo ──────────────────────────────────────────
public sealed class CrearGrupoManejoEndpoint(IPotreroService svc) : Endpoint<CrearGrupoManejoRequest, GrupoManejoResponse>
{
    public override void Configure()
    {
        Post("api/potreros/grupos");
    }

    public override async Task HandleAsync(CrearGrupoManejoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearGrupoAsync(req, ct), ct);
}

public sealed class ListarGruposManejoEndpoint(IPotreroService svc) : EndpointWithoutRequest<IReadOnlyList<GrupoManejoResponse>>
{
    public override void Configure()
    {
        Get("api/potreros/grupos");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarGruposAsync(ct), ct);
}

// ── Animales por Potrero ───────────────────────────────────────
public sealed class AsignarAnimalPotreroEndpoint(IPotreroService svc) : Endpoint<AsignarAnimalPotreroRequest, AnimalPotreroResponse>
{
    public override void Configure()
    {
        Post("api/potreros/asignaciones");
    }

    public override async Task HandleAsync(AsignarAnimalPotreroRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.AsignarAnimalAsync(req, ct), ct);
}

public sealed class CerrarAsignacionPotreroEndpoint(IPotreroService svc) : Endpoint<CerrarAnimalPotreroRequest, AnimalPotreroResponse>
{
    public override void Configure()
    {
        Patch("api/potreros/asignaciones/{id:int}/cerrar");
    }

    public override async Task HandleAsync(CerrarAnimalPotreroRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CerrarAsignacionAsync(Route<int>("id"), req, ct), ct);
}

public sealed class ListarAnimalesPorPotreroEndpoint(IPotreroService svc) : EndpointWithoutRequest<IReadOnlyList<AnimalPotreroResponse>>
{
    public override void Configure()
    {
        Get("api/potreros/{idPotrero:int}/animales");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAnimalesPorPotreroAsync(Route<int>("idPotrero"), ct), ct);
}

// ── Acumulación de Insumos ─────────────────────────────────────
public sealed class RegistrarAcumulacionInsumoEndpoint(IPotreroService svc) : Endpoint<RegistrarAcumulacionInsumoRequest, AcumulacionInsumoPotreroResponse>
{
    public override void Configure()
    {
        Post("api/potreros/acumulaciones");
    }

    public override async Task HandleAsync(RegistrarAcumulacionInsumoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarAcumulacionAsync(req, ct), ct);
}

public sealed class ListarAcumulacionesPorPotreroEndpoint(IPotreroService svc) : EndpointWithoutRequest<IReadOnlyList<AcumulacionInsumoPotreroResponse>>
{
    public override void Configure()
    {
        Get("api/potreros/{idPotrero:int}/acumulaciones");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAcumulacionesPorPotreroAsync(Route<int>("idPotrero"), ct), ct);
}
