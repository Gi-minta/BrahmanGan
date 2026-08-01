using FastEndpoints;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Endpoints.Animales;

public sealed class RegistrarAnimalEndpoint(IAnimalService svc) : Endpoint<CrearAnimalRequest, AnimalResponse>
{
    public override void Configure()
    {
        Post("api/animales");
        Permissions("Inventario:Crear");
    }

    public override async Task HandleAsync(CrearAnimalRequest req, CancellationToken ct)
    {
        var r = await svc.RegistrarAsync(req, ct);
        await Send.CreatedAtAsync<ObtenerAnimalEndpoint>(new { id = r.Id }, r, cancellation: ct);
    }
}

public sealed class ObtenerAnimalEndpoint(IAnimalService svc) : EndpointWithoutRequest<AnimalResponse>
{
    public override void Configure()
    {
        Get("api/animales/{id:int}");
        Permissions("Inventario:Leer");
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

public sealed class ListarAnimalesActivosEndpoint(IAnimalService svc) : EndpointWithoutRequest<IReadOnlyList<AnimalResponse>>
{
    public override void Configure()
    {
        Get("api/animales/activos");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarActivosAsync(ct), ct);
}

public sealed class ListarAnimalesPorFincaEndpoint(IAnimalService svc) : EndpointWithoutRequest<IReadOnlyList<AnimalResponse>>
{
    public override void Configure()
    {
        Get("api/animales/finca/{idFinca:int}");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarPorFincaAsync(Route<int>("idFinca"), ct), ct);
}

public sealed class CambiarEstadoAnimalEndpoint(IAnimalService svc) : Endpoint<CambiarEstadoAnimalRequest>
{
    public override void Configure()
    {
        Put("api/animales/{id:int}/estado");
        Permissions("Inventario:Editar");
    }

    public override async Task HandleAsync(CambiarEstadoAnimalRequest req, CancellationToken ct)
    {
        await svc.CambiarEstadoAsync(Route<int>("id"), req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class TrasladarAnimalEndpoint(IAnimalService svc) : Endpoint<TrasladarAnimalRequest>
{
    public override void Configure()
    {
        Put("api/animales/{id:int}/trasladar");
        Permissions("Inventario:Editar");
    }

    public override async Task HandleAsync(TrasladarAnimalRequest req, CancellationToken ct)
    {
        await svc.TrasladarAsync(Route<int>("id"), req, ct);
        await Send.NoContentAsync(ct);
    }
}

public sealed class HistorialAnimalEndpoint(IAnimalService svc) : EndpointWithoutRequest<IReadOnlyList<HistorialAnimalResponse>>
{
    public override void Configure()
    {
        Get("api/animales/{id:int}/historial");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarHistorialAsync(Route<int>("id"), ct), ct);
}

public sealed class MovimientosAnimalEndpoint(IAnimalService svc) : EndpointWithoutRequest<IReadOnlyList<MovimientoAnimalResponse>>
{
    public override void Configure()
    {
        Get("api/animales/{id:int}/movimientos");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(await svc.ListarMovimientosAsync(Route<int>("id"), ct), ct);
}

public sealed class CrearPedigriEndpoint(IAnimalService svc) : Endpoint<CrearPedigriRequest, PedigriResponse>
{
    public override void Configure()
    {
        Post("api/animales/pedigri");
        Permissions("Inventario:Crear");
    }

    public override async Task HandleAsync(CrearPedigriRequest req, CancellationToken ct)
        => await Send.OkAsync(await svc.CrearPedigriAsync(req, ct), ct);
}

public sealed class ObtenerPedigriEndpoint(IAnimalService svc) : EndpointWithoutRequest<PedigriResponse>
{
    public override void Configure()
    {
        Get("api/animales/{id:int}/pedigri");
        Permissions("Inventario:Leer");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var r = await svc.ObtenerPedigriAsync(Route<int>("id"), ct);
        if (r is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync(r, ct);
    }
}
