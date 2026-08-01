using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Reproduccion;

public sealed class IniciarGestacionEndpoint(IGestacionService svc) : Endpoint<IniciarGestacionRequest, GestacionResponse>
{
    public override void Configure()
    {
        Post("api/gestaciones");
        Permissions("Reproduccion:Crear");
    }

    public override async Task HandleAsync(IniciarGestacionRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.IniciarAsync(req, ct), ct);
}

public sealed class ObtenerGestacionEndpoint(IGestacionService svc) : EndpointWithoutRequest<GestacionResponse>
{
    public override void Configure()
    {
        Get("api/gestaciones/{id:int}");
        Permissions("Reproduccion:Leer");
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

public sealed class RegistrarPartoEndpoint(IGestacionService svc) : Endpoint<RegistrarPartoRequest>
{
    public override void Configure()
    {
        Put("api/gestaciones/{id:int}/parto");
        Permissions("Reproduccion:Editar");
    }

    public override async Task HandleAsync(RegistrarPartoRequest req, CancellationToken ct)
    {
        await svc.RegistrarPartoAsync(Route<int>("id"), req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class RegistrarAbortoEndpoint(IGestacionService svc) : Endpoint<RegistrarAbortoRequest>
{
    public override void Configure()
    {
        Put("api/gestaciones/{id:int}/aborto");
        Permissions("Reproduccion:Editar");
    }

    public override async Task HandleAsync(RegistrarAbortoRequest req, CancellationToken ct)
    {
        await svc.RegistrarAbortoAsync(Route<int>("id"), req, ct);
        await Send.NoContentAsync(ct);
    }
}
