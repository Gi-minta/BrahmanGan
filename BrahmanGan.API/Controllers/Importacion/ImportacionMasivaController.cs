using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using BrahmanGan.Application.ImportacionMasiva;
using BrahmanGan.Application.ImportacionMasiva.Servicios;

namespace BrahmanGan.API.Controllers.Importacion;

[ApiController]
[Route("api/importacion")]
public class ImportacionMasivaController : ControllerBase
{
    // ── Inventario ─────────────────────────────────────────────────
    [HttpPost("razas")]
    public Task<ActionResult<ImportResult>> ImportarRazas(
        IFormFile archivo,
        [FromServices] ImportRazasService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("animales")]
    public Task<ActionResult<ImportResult>> ImportarAnimales(
        IFormFile archivo,
        [FromServices] ImportAnimalesService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("pesajes")]
    public Task<ActionResult<ImportResult>> ImportarPesajes(
        IFormFile archivo,
        [FromServices] ImportPesajesService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Fincas ─────────────────────────────────────────────────────
    [HttpPost("fincas")]
    public Task<ActionResult<ImportResult>> ImportarFincas(
        IFormFile archivo,
        [FromServices] ImportFincasService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("potreros")]
    public Task<ActionResult<ImportResult>> ImportarPotreros(
        IFormFile archivo,
        [FromServices] ImportPotrerosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Reproducción ───────────────────────────────────────────────
    [HttpPost("servicios-reproductivos")]
    public Task<ActionResult<ImportResult>> ImportarServiciosReproductivos(
        IFormFile archivo,
        [FromServices] ImportServiciosReproductivosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("gestaciones")]
    public Task<ActionResult<ImportResult>> ImportarGestaciones(
        IFormFile archivo,
        [FromServices] ImportGestacionesService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Sanidad ────────────────────────────────────────────────────
    [HttpPost("medicamentos")]
    public Task<ActionResult<ImportResult>> ImportarMedicamentos(
        IFormFile archivo,
        [FromServices] ImportMedicamentosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("vacunaciones")]
    public Task<ActionResult<ImportResult>> ImportarVacunaciones(
        IFormFile archivo,
        [FromServices] ImportVacunacionesService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Leche ──────────────────────────────────────────────────────
    [HttpPost("control-leche")]
    public Task<ActionResult<ImportResult>> ImportarControlLeche(
        IFormFile archivo,
        [FromServices] ImportControlLecheService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("produccion-leche")]
    public Task<ActionResult<ImportResult>> ImportarProduccionLeche(
        IFormFile archivo,
        [FromServices] ImportProduccionLecheService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("ventas-leche")]
    public Task<ActionResult<ImportResult>> ImportarVentasLeche(
        IFormFile archivo,
        [FromServices] ImportVentasLecheService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Comercial ──────────────────────────────────────────────────
    [HttpPost("clientes")]
    public Task<ActionResult<ImportResult>> ImportarClientes(
        IFormFile archivo,
        [FromServices] ImportClientesService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Costos ─────────────────────────────────────────────────────
    [HttpPost("centros-costo")]
    public Task<ActionResult<ImportResult>> ImportarCentrosCosto(
        IFormFile archivo,
        [FromServices] ImportCentrosCostoService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("gastos")]
    public Task<ActionResult<ImportResult>> ImportarGastos(
        IFormFile archivo,
        [FromServices] ImportGastosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("ingresos")]
    public Task<ActionResult<ImportResult>> ImportarIngresos(
        IFormFile archivo,
        [FromServices] ImportIngresosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Almacén ────────────────────────────────────────────────────
    [HttpPost("insumos")]
    public Task<ActionResult<ImportResult>> ImportarInsumos(
        IFormFile archivo,
        [FromServices] ImportInsumosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Equipos ────────────────────────────────────────────────────
    [HttpPost("maquinaria")]
    public Task<ActionResult<ImportResult>> ImportarMaquinaria(
        IFormFile archivo,
        [FromServices] ImportMaquinariaService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Nómina ─────────────────────────────────────────────────────
    [HttpPost("trabajadores")]
    public Task<ActionResult<ImportResult>> ImportarTrabajadores(
        IFormFile archivo,
        [FromServices] ImportTrabajadoresService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("pagos-jornal")]
    public Task<ActionResult<ImportResult>> ImportarPagosJornal(
        IFormFile archivo,
        [FromServices] ImportPagosJornalService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Trazabilidad ───────────────────────────────────────────────
    [HttpPost("registros-ica")]
    public Task<ActionResult<ImportResult>> ImportarRegistrosICA(
        IFormFile archivo,
        [FromServices] ImportRegistrosICAService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Sostenibilidad ─────────────────────────────────────────────
    [HttpPost("captura-carbono")]
    public Task<ActionResult<ImportResult>> ImportarCapturaCarbono(
        IFormFile archivo,
        [FromServices] ImportCapturaCarbonoService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("consumo-agua")]
    public Task<ActionResult<ImportResult>> ImportarConsumoAgua(
        IFormFile archivo,
        [FromServices] ImportConsumoAguaService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Alimentación ───────────────────────────────────────────────
    [HttpPost("planes-alimentacion")]
    public Task<ActionResult<ImportResult>> ImportarPlanesAlimentacion(
        IFormFile archivo,
        [FromServices] ImportPlanesAlimentacionService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("detalles-alimentacion")]
    public Task<ActionResult<ImportResult>> ImportarDetallesAlimentacion(
        IFormFile archivo,
        [FromServices] ImportDetallesAlimentacionService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Pastoreo ───────────────────────────────────────────────────
    [HttpPost("planes-pastoreo")]
    public Task<ActionResult<ImportResult>> ImportarPlanesPastoreo(
        IFormFile archivo,
        [FromServices] ImportPlanesPastoreoService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Sanidad (nuevos) ───────────────────────────────────────────
    [HttpPost("complementos")]
    public Task<ActionResult<ImportResult>> ImportarComplementos(
        IFormFile archivo,
        [FromServices] ImportComplementosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("desparasitaciones")]
    public Task<ActionResult<ImportResult>> ImportarDesparasitaciones(
        IFormFile archivo,
        [FromServices] ImportDesparasitacionesService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("aplicar-controles")]
    public Task<ActionResult<ImportResult>> ImportarControlesPreventivos(
        IFormFile archivo,
        [FromServices] ImportControlesPreventivosService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Leche (nuevos) ─────────────────────────────────────────────
    [HttpPost("lactancias")]
    public Task<ActionResult<ImportResult>> ImportarLactancias(
        IFormFile archivo,
        [FromServices] ImportLactanciasService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    [HttpPost("calidad-leche")]
    public Task<ActionResult<ImportResult>> ImportarCalidadLeche(
        IFormFile archivo,
        [FromServices] ImportCalidadLecheService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Reproducción (nuevos) ──────────────────────────────────────
    [HttpPost("semen")]
    public Task<ActionResult<ImportResult>> ImportarSemen(
        IFormFile archivo,
        [FromServices] ImportSemenService svc,
        CancellationToken ct)
        => Ejecutar(archivo, svc.ImportarAsync, ct);

    // ── Plantillas ─────────────────────────────────────────────────
    [HttpGet("plantilla/{modulo}")]
    public IActionResult DescargarPlantilla(string modulo)
    {
        try
        {
            var encabezados = Plantillas.ObtenerEncabezados(modulo);
            var bytes = Encoding.UTF8.GetBytes(encabezados + "\n");
            return File(bytes, "text/csv", $"plantilla_{modulo}.csv");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message, modulosDisponibles = Plantillas.Modulos });
        }
    }

    [HttpGet("modulos")]
    public IActionResult ListarModulos() => Ok(Plantillas.Modulos);

    // ── Helpers ────────────────────────────────────────────────────
    private async Task<ActionResult<ImportResult>> Ejecutar(
        IFormFile archivo,
        Func<Stream, CancellationToken, Task<ImportResult>> importar,
        CancellationToken ct)
    {
        if (archivo is null || archivo.Length == 0)
            return BadRequest(new { mensaje = "Debe adjuntar un archivo CSV." });

        if (archivo.Length > 10 * 1024 * 1024)
            return BadRequest(new { mensaje = "El archivo no puede superar 10 MB." });

        using var stream = archivo.OpenReadStream();
        var resultado = await importar(stream, ct);
        return Ok(resultado);
    }
}
