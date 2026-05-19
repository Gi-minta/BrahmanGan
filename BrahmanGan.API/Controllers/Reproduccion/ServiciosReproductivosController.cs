using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Reproduccion;

[ApiController]
[Route("api/servicios-reproductivos")]
public class ServiciosReproductivosController : ControllerBase
{
    private readonly IServicioReproductivoService _svc;
    public ServiciosReproductivosController(IServicioReproductivoService svc) => _svc = svc;

    [HttpPost("monta")]
    public async Task<ActionResult<ServicioResponse>> RegistrarMonta([FromBody] RegistrarMontaRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarMontaAsync(req, ct));

    [HttpPost("ia")]
    public async Task<ActionResult<ServicioResponse>> RegistrarIa([FromBody] RegistrarIaRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarIaAsync(req, ct));

    [HttpPut("{id:int}/confirmar")]
    public async Task<IActionResult> Confirmar(int id, [FromBody] ConfirmarServicioRequest req, CancellationToken ct)
    {
        await _svc.ConfirmarAsync(id, req, ct);
        return NoContent();
    }

    [HttpGet("hembra/{idHembra:int}")]
    public async Task<ActionResult<IReadOnlyList<ServicioResponse>>> ListarPorHembra(int idHembra, CancellationToken ct)
        => Ok(await _svc.ListarPorHembraAsync(idHembra, ct));

    // ── Semen ──────────────────────────────────────────────────────
    [HttpPost("semen")]
    public async Task<ActionResult<SemenResponse>> CrearSemen([FromBody] CrearSemenRequest req, CancellationToken ct)
        => Ok(await _svc.CrearSemenAsync(req, ct));

    [HttpGet("semen")]
    public async Task<ActionResult<IReadOnlyList<SemenResponse>>> ListarSemen(CancellationToken ct)
        => Ok(await _svc.ListarSemenAsync(ct));

    [HttpGet("semen/{id:int}")]
    public async Task<ActionResult<SemenResponse>> ObtenerSemen(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerSemenAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpPatch("semen/stock")]
    public async Task<ActionResult<SemenResponse>> AjustarStock([FromBody] AjustarStockSemenRequest req, CancellationToken ct)
        => Ok(await _svc.AjustarStockSemenAsync(req, ct));

    // ── Nacimientos ────────────────────────────────────────────────
    [HttpGet("nacimientos/{id:int}")]
    public async Task<ActionResult<NacimientoResponse>> ObtenerNacimiento(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerNacimientoAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet("nacimientos/gestacion/{idGestacion:int}")]
    public async Task<ActionResult<IReadOnlyList<NacimientoResponse>>> ListarNacimientos(int idGestacion, CancellationToken ct)
        => Ok(await _svc.ListarNacimientosPorGestacionAsync(idGestacion, ct));
}
