using System.Text;
using FastEndpoints;
using BrahmanGan.Application.ImportacionMasiva;
using BrahmanGan.Application.ImportacionMasiva.Servicios;

namespace BrahmanGan.API.Endpoints.Importacion;

public sealed class ImportArchivoRequest
{
    public IFormFile Archivo { get; set; } = null!;
}

/// <summary>
/// Base para los endpoints de importación masiva vía CSV.
/// Cada endpoint concreto define su ruta y delega en su servicio de importación.
/// </summary>
public abstract class ImportacionEndpointBase : Endpoint<ImportArchivoRequest, ImportResult>
{
    protected abstract string Modulo { get; }

    public override void Configure()
    {
        Post($"api/importacion/{Modulo}");
        AllowFileUploads();
        // Reservada a administradores en vez de a un permiso por módulo: la base es
        // genérica y no conoce a qué ModuloSistema corresponde su ruta, y una carga
        // masiva puede crear miles de registros de una sola vez.
        Policies("Administrador");
    }

    protected abstract Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct);

    public override async Task HandleAsync(ImportArchivoRequest req, CancellationToken ct)
    {
        if (req.Archivo is null || req.Archivo.Length == 0)
        {
            await Send.ResultAsync(Results.BadRequest(new { mensaje = "Debe adjuntar un archivo CSV." }));
            return;
        }

        if (req.Archivo.Length > 10 * 1024 * 1024)
        {
            await Send.ResultAsync(Results.BadRequest(new { mensaje = "El archivo no puede superar 10 MB." }));
            return;
        }

        using var stream = req.Archivo.OpenReadStream();
        await Send.OkAsync(await ImportarAsync(stream, ct), ct);
    }
}

// ── Inventario ─────────────────────────────────────────────────
public sealed class ImportarRazasEndpoint(ImportRazasService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "razas";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarAnimalesEndpoint(ImportAnimalesService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "animales";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarPesajesEndpoint(ImportPesajesService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "pesajes";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Fincas ─────────────────────────────────────────────────────
public sealed class ImportarFincasEndpoint(ImportFincasService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "fincas";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarPotrerosEndpoint(ImportPotrerosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "potreros";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Reproducción ───────────────────────────────────────────────
public sealed class ImportarServiciosReproductivosEndpoint(ImportServiciosReproductivosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "servicios-reproductivos";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarGestacionesEndpoint(ImportGestacionesService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "gestaciones";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarSemenEndpoint(ImportSemenService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "semen";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Sanidad ────────────────────────────────────────────────────
public sealed class ImportarMedicamentosEndpoint(ImportMedicamentosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "medicamentos";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarVacunacionesEndpoint(ImportVacunacionesService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "vacunaciones";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarComplementosEndpoint(ImportComplementosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "complementos";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarDesparasitacionesEndpoint(ImportDesparasitacionesService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "desparasitaciones";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarControlesPreventivosEndpoint(ImportControlesPreventivosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "aplicar-controles";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Leche ──────────────────────────────────────────────────────
public sealed class ImportarControlLecheEndpoint(ImportControlLecheService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "control-leche";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarProduccionLecheEndpoint(ImportProduccionLecheService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "produccion-leche";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarVentasLecheEndpoint(ImportVentasLecheService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "ventas-leche";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarLactanciasEndpoint(ImportLactanciasService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "lactancias";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarCalidadLecheEndpoint(ImportCalidadLecheService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "calidad-leche";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Comercial ──────────────────────────────────────────────────
public sealed class ImportarClientesEndpoint(ImportClientesService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "clientes";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Costos ─────────────────────────────────────────────────────
public sealed class ImportarCentrosCostoEndpoint(ImportCentrosCostoService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "centros-costo";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarGastosEndpoint(ImportGastosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "gastos";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarIngresosEndpoint(ImportIngresosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "ingresos";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Almacén ────────────────────────────────────────────────────
public sealed class ImportarInsumosEndpoint(ImportInsumosService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "insumos";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Equipos ────────────────────────────────────────────────────
public sealed class ImportarMaquinariaEndpoint(ImportMaquinariaService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "maquinaria";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Nómina ─────────────────────────────────────────────────────
public sealed class ImportarTrabajadoresEndpoint(ImportTrabajadoresService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "trabajadores";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarPagosJornalEndpoint(ImportPagosJornalService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "pagos-jornal";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Trazabilidad ───────────────────────────────────────────────
public sealed class ImportarRegistrosICAEndpoint(ImportRegistrosICAService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "registros-ica";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Sostenibilidad ─────────────────────────────────────────────
public sealed class ImportarCapturaCarbonoEndpoint(ImportCapturaCarbonoService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "captura-carbono";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarConsumoAguaEndpoint(ImportConsumoAguaService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "consumo-agua";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Alimentación ───────────────────────────────────────────────
public sealed class ImportarPlanesAlimentacionEndpoint(ImportPlanesAlimentacionService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "planes-alimentacion";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

public sealed class ImportarDetallesAlimentacionEndpoint(ImportDetallesAlimentacionService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "detalles-alimentacion";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Pastoreo ───────────────────────────────────────────────────
public sealed class ImportarPlanesPastoreoEndpoint(ImportPlanesPastoreoService svc) : ImportacionEndpointBase
{
    protected override string Modulo => "planes-pastoreo";
    protected override Task<ImportResult> ImportarAsync(Stream archivo, CancellationToken ct) => svc.ImportarAsync(archivo, ct);
}

// ── Plantillas ─────────────────────────────────────────────────
public sealed class DescargarPlantillaEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("api/importacion/plantilla/{modulo}");
        // Mismo criterio que la carga: la plantilla solo la necesita quien va a importar.
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var modulo = Route<string>("modulo")!;
        try
        {
            var encabezados = Plantillas.ObtenerEncabezados(modulo);
            var bytes = Encoding.UTF8.GetBytes(encabezados + "\n");
            await Send.BytesAsync(bytes, fileName: $"plantilla_{modulo}.csv", contentType: "text/csv", cancellation: ct);
        }
        catch (ArgumentException ex)
        {
            await Send.ResultAsync(Results.BadRequest(new { mensaje = ex.Message, modulosDisponibles = Plantillas.Modulos }));
        }
    }
}

public sealed class ListarModulosImportacionEndpoint : EndpointWithoutRequest<IReadOnlyCollection<string>>
{
    public override void Configure()
    {
        Get("api/importacion/modulos");
        Policies("Administrador");
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await Send.OkAsync(Plantillas.Modulos, ct);
}
