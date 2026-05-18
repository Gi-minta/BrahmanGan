using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Sostenibilidad;

[ApiController]
[Route("api/sostenibilidad")]
public class SostenibilidadController : ControllerBase
{
    private readonly ISostenibilidadService _svc;
    public SostenibilidadController(ISostenibilidadService svc) => _svc = svc;

    // ── Captura de carbono ──────────────────────────────────────
    [HttpPost("carbono")]
    public async Task<ActionResult<CapturaCarbonoResponse>> RegistrarCaptura(
        [FromBody] RegistrarCapturaCarbonoRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarCapturaAsync(req, ct));

    [HttpGet("carbono/finca/{idFinca:int}")]
    public async Task<ActionResult<IReadOnlyList<CapturaCarbonoResponse>>> ListarCapturas(int idFinca, CancellationToken ct)
        => Ok(await _svc.ListarCapturasPorFincaAsync(idFinca, ct));

    // ── Consumo de agua ─────────────────────────────────────────
    [HttpPost("agua")]
    public async Task<ActionResult<ConsumoAguaResponse>> RegistrarConsumoAgua(
        [FromBody] RegistrarConsumoAguaRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarConsumoAguaAsync(req, ct));

    [HttpGet("agua/finca/{idFinca:int}")]
    public async Task<ActionResult<IReadOnlyList<ConsumoAguaResponse>>> ListarConsumoAgua(int idFinca, CancellationToken ct)
        => Ok(await _svc.ListarConsumoAguaPorFincaAsync(idFinca, ct));

    // ── Eventos medioambientales ────────────────────────────────
    [HttpPost("eventos")]
    public async Task<ActionResult<EventoMedioambientalResponse>> RegistrarEvento(
        [FromBody] RegistrarEventoMedioambientalRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarEventoAsync(req, ct));

    [HttpGet("eventos/finca/{idFinca:int}")]
    public async Task<ActionResult<IReadOnlyList<EventoMedioambientalResponse>>> ListarEventos(int idFinca, CancellationToken ct)
        => Ok(await _svc.ListarEventosPorFincaAsync(idFinca, ct));
}
