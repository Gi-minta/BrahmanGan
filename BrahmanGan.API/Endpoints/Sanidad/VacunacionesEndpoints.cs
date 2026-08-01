using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Sanidad;

public sealed class AplicarVacunaEndpoint(IVacunacionService svc) : Endpoint<AplicarVacunaRequest, VacunacionResponse>
{
    public override void Configure()
    {
        Post("api/vacunaciones");
        Permissions("Sanidad:Crear");
    }

    public override async Task HandleAsync(AplicarVacunaRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.AplicarAsync(req, ct), ct);
}

public sealed class ListarVacunacionesPorAnimalEndpoint(IVacunacionService svc) : EndpointWithoutRequest<IReadOnlyList<VacunacionResponse>>
{
    public override void Configure()
    {
        Get("api/vacunaciones/animal/{idAnimal:int}");
        Permissions("Sanidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorAnimalAsync(Route<int>("idAnimal"), ct), ct);
}

public sealed class AlertasVacunacionEndpoint(IVacunacionService svc) : EndpointWithoutRequest<IReadOnlyList<VacunacionResponse>>
{
    public override void Configure()
    {
        Get("api/vacunaciones/alertas");
        Permissions("Sanidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAlertasAsync(Query<int?>("dias", isRequired: false) ?? 7, ct), ct);
}

public sealed class AplicarDesparasitacionEndpoint(IVacunacionService svc) : Endpoint<AplicarDesparasitacionRequest, DesparasitacionResponse>
{
    public override void Configure()
    {
        Post("api/vacunaciones/desparasitacion");
        Permissions("Sanidad:Crear");
    }

    public override async Task HandleAsync(AplicarDesparasitacionRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.AplicarDesparasitacionAsync(req, ct), ct);
}

public sealed class ListarDesparasitacionesPorAnimalEndpoint(IVacunacionService svc) : EndpointWithoutRequest<IReadOnlyList<DesparasitacionResponse>>
{
    public override void Configure()
    {
        Get("api/vacunaciones/desparasitacion/animal/{idAnimal:int}");
        Permissions("Sanidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarDesparasitacionesPorAnimalAsync(Route<int>("idAnimal"), ct), ct);
}
