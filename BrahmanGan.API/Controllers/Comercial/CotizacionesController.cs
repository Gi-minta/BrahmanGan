using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Comercial;

[ApiController]
[Route("api/cotizaciones")]
public class CotizacionesController : ControllerBase
{
    private readonly ICotizacionVentaService _svc;
    public CotizacionesController(ICotizacionVentaService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<CotizacionResponse>> Crear([FromBody] CrearCotizacionRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CotizacionResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpPost("{id:int}/detalle")]
    public async Task<IActionResult> AgregarDetalle(int id, [FromBody] AgregarDetalleCotizacionRequest req, CancellationToken ct)
    { await _svc.AgregarDetalleAsync(id, req, ct); return NoContent(); }

    [HttpPut("{id:int}/aprobar")]
    public async Task<IActionResult> Aprobar(int id, CancellationToken ct)
    { await _svc.AprobarAsync(id, ct); return NoContent(); }

    [HttpPut("{id:int}/rechazar")]
    public async Task<IActionResult> Rechazar(int id, CancellationToken ct)
    { await _svc.RechazarAsync(id, ct); return NoContent(); }
}
