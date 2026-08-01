using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Nomina;

public sealed class ContratarTrabajadorEndpoint(ITrabajadorService svc) : Endpoint<ContratarTrabajadorRequest, TrabajadorResponse>
{
    public override void Configure()
    {
        Post("api/trabajadores");
        Permissions("Nomina:Crear");
    }

    public override async Task HandleAsync(ContratarTrabajadorRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.ContratarAsync(req, ct), ct);
}

public sealed class ObtenerTrabajadorEndpoint(ITrabajadorService svc) : EndpointWithoutRequest<TrabajadorResponse>
{
    public override void Configure()
    {
        Get("api/trabajadores/{id:int}");
        Permissions("Nomina:Leer");
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

public sealed class ListarTrabajadoresEndpoint(ITrabajadorService svc) : EndpointWithoutRequest<IReadOnlyList<TrabajadorResponse>>
{
    public override void Configure()
    {
        Get("api/trabajadores");
        Permissions("Nomina:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}

public sealed class RegistrarPagoJornalEndpoint(ITrabajadorService svc) : Endpoint<RegistrarPagoJornalRequest, PagoJornalResponse>
{
    public override void Configure()
    {
        Post("api/trabajadores/pagos");
        Permissions("Nomina:Crear");
    }

    public override async Task HandleAsync(RegistrarPagoJornalRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarPagoAsync(req, ct), ct);
}

public sealed class ListarPagosTrabajadorEndpoint(ITrabajadorService svc) : EndpointWithoutRequest<IReadOnlyList<PagoJornalResponse>>
{
    public override void Configure()
    {
        Get("api/trabajadores/{id:int}/pagos");
        Permissions("Nomina:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPagosAsync(Route<int>("id"), ct), ct);
}
