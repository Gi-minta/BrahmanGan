using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Leche;

[ApiController]
[Route("api/control-leche")]
public class ControlLecheController : ControllerBase
{
    private readonly IControlLecheService _svc;
    public ControlLecheController(IControlLecheService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<ControlLecheResponse>> Registrar([FromBody] RegistrarControlLecheRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarAsync(req, ct));

    [HttpGet("animal/{idAnimal:int}")]
    public async Task<ActionResult<IReadOnlyList<ControlLecheResponse>>> ListarPorAnimal(int idAnimal,
        [FromQuery] DateOnly desde, [FromQuery] DateOnly hasta, CancellationToken ct)
        => Ok(await _svc.ListarPorAnimalAsync(idAnimal, desde, hasta, ct));

    // ── Parámetros Lactancia ───────────────────────────────────────
    [HttpPost("lactancias")]
    public async Task<ActionResult<ParametroLactanciaResponse>> IniciarLactancia([FromBody] IniciarParametroLactanciaRequest req, CancellationToken ct)
        => Ok(await _svc.IniciarLactanciaAsync(req, ct));

    [HttpPatch("lactancias/{id:int}/cerrar")]
    public async Task<ActionResult<ParametroLactanciaResponse>> CerrarLactancia(int id, [FromBody] CerrarParametroLactanciaRequest req, CancellationToken ct)
        => Ok(await _svc.CerrarLactanciaAsync(id, req, ct));

    [HttpGet("lactancias/animal/{idAnimal:int}")]
    public async Task<ActionResult<IReadOnlyList<ParametroLactanciaResponse>>> ListarLactancias(int idAnimal, CancellationToken ct)
        => Ok(await _svc.ListarLactanciasPorAnimalAsync(idAnimal, ct));

    // ── Calidad Leche ──────────────────────────────────────────────
    [HttpPost("calidad")]
    public async Task<ActionResult<CalidadLecheResponse>> RegistrarCalidad([FromBody] RegistrarCalidadLecheRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarCalidadAsync(req, ct));

    [HttpGet("calidad")]
    public async Task<ActionResult<IReadOnlyList<CalidadLecheResponse>>> ListarCalidad([FromQuery] DateOnly desde, [FromQuery] DateOnly hasta, CancellationToken ct)
        => Ok(await _svc.ListarCalidadPorFechaAsync(desde, hasta, ct));
}
