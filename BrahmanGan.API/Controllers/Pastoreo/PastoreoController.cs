using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Pastoreo;

[ApiController]
[Route("api/pastoreo")]
public class PastoreoController : ControllerBase
{
    private readonly IPastoreoService _svc;
    public PastoreoController(IPastoreoService svc) => _svc = svc;

    [HttpPost("planes")]
    public async Task<ActionResult<PlanPastoreoResponse>> CrearPlan(
        [FromBody] CrearPlanPastoreoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearPlanAsync(req, ct));

    [HttpGet("planes/{id:int}")]
    public async Task<ActionResult<PlanPastoreoResponse>> ObtenerPlan(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerPlanAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet("planes")]
    public async Task<ActionResult<IReadOnlyList<PlanPastoreoResponse>>> ListarPlanes(CancellationToken ct)
        => Ok(await _svc.ListarPlanesAsync(ct));

    [HttpGet("planes/potrero/{idPotrero:int}")]
    public async Task<ActionResult<IReadOnlyList<PlanPastoreoResponse>>> ListarPorPotrero(int idPotrero, CancellationToken ct)
        => Ok(await _svc.ListarPorPotreroAsync(idPotrero, ct));

    [HttpPatch("planes/{id:int}/finalizar")]
    public async Task<IActionResult> FinalizarPlan(int id, [FromQuery] DateOnly fechaFin, CancellationToken ct)
    {
        await _svc.FinalizarPlanAsync(id, fechaFin, ct);
        return NoContent();
    }
}
