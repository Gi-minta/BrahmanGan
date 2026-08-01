using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Sanidad;

public sealed class CrearMedicamentoEndpoint(IMedicamentoService svc) : Endpoint<CrearMedicamentoRequest, MedicamentoResponse>
{
    public override void Configure()
    {
        Post("api/medicamentos");
        Permissions("Sanidad:Crear");
    }

    public override async Task HandleAsync(CrearMedicamentoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ListarMedicamentosEndpoint(IMedicamentoService svc) : EndpointWithoutRequest<IReadOnlyList<MedicamentoResponse>>
{
    public override void Configure()
    {
        Get("api/medicamentos");
        Permissions("Sanidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}

public sealed class CrearControlPreventivoEndpoint(IMedicamentoService svc) : Endpoint<CrearControlPreventivoRequest, ControlPreventivoResponse>
{
    public override void Configure()
    {
        Post("api/medicamentos/controles");
        Permissions("Sanidad:Crear");
    }

    public override async Task HandleAsync(CrearControlPreventivoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearControlAsync(req, ct), ct);
}

public sealed class ListarControlesPreventivosEndpoint(IMedicamentoService svc) : EndpointWithoutRequest<IReadOnlyList<ControlPreventivoResponse>>
{
    public override void Configure()
    {
        Get("api/medicamentos/controles");
        Permissions("Sanidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarControlesAsync(ct), ct);
}

public sealed class AplicarControlPreventivoEndpoint(IMedicamentoService svc) : Endpoint<AplicarControlPreventivoRequest, HistorialPreventivoResponse>
{
    public override void Configure()
    {
        Post("api/medicamentos/controles/aplicar");
        Permissions("Sanidad:Crear");
    }

    public override async Task HandleAsync(AplicarControlPreventivoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.AplicarControlAsync(req, ct), ct);
}

public sealed class HistorialPreventivoEndpoint(IMedicamentoService svc) : EndpointWithoutRequest<IReadOnlyList<HistorialPreventivoResponse>>
{
    public override void Configure()
    {
        Get("api/medicamentos/controles/historial/{idAnimal:int}");
        Permissions("Sanidad:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarHistorialPreventivoAsync(Route<int>("idAnimal"), ct), ct);
}
