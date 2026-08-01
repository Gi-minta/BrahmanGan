using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Almacen;

public sealed class CrearInsumoEndpoint(IInsumoService svc) : Endpoint<CrearInsumoRequest, InsumoResponse>
{
    public override void Configure()
    {
        Post("api/insumos");
        Permissions("Almacen:Crear");
    }

    public override async Task HandleAsync(CrearInsumoRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearAsync(req, ct), ct);
}

public sealed class ObtenerInsumoEndpoint(IInsumoService svc) : EndpointWithoutRequest<InsumoResponse>
{
    public override void Configure()
    {
        Get("api/insumos/{id:int}");
        Permissions("Almacen:Leer");
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

public sealed class ListarInsumosEndpoint(IInsumoService svc) : EndpointWithoutRequest<IReadOnlyList<InsumoResponse>>
{
    public override void Configure()
    {
        Get("api/insumos");
        Permissions("Almacen:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarAsync(ct), ct);
}

public sealed class ListarInsumosBajoMinimoEndpoint(IInsumoService svc) : EndpointWithoutRequest<IReadOnlyList<InsumoResponse>>
{
    public override void Configure()
    {
        Get("api/insumos/bajo-minimo");
        Permissions("Almacen:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarBajoMinimoAsync(ct), ct);
}

public sealed class RegistrarMovimientoKardexEndpoint(IInsumoService svc) : Endpoint<RegistrarMovimientoKardexRequest, KardexInsumoResponse>
{
    public override void Configure()
    {
        Post("api/insumos/movimiento");
        Permissions("Almacen:Crear");
    }

    public override async Task HandleAsync(RegistrarMovimientoKardexRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.RegistrarMovimientoAsync(req, ct), ct);
}

public sealed class KardexInsumoEndpoint(IInsumoService svc) : EndpointWithoutRequest<IReadOnlyList<KardexInsumoResponse>>
{
    public override void Configure()
    {
        Get("api/insumos/{id:int}/kardex");
        Permissions("Almacen:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarKardexAsync(Route<int>("id"), ct), ct);
}
