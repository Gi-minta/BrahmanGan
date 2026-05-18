using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Alimentacion;

[ApiController]
[Route("api/alimentacion")]
public class AlimentacionController : ControllerBase
{
    private readonly IAlimentacionService _svc;
    public AlimentacionController(IAlimentacionService svc) => _svc = svc;

    [HttpPost("planes")]
    public async Task<ActionResult<PlanAlimentacionResponse>> CrearPlan(
        [FromBody] CrearPlanAlimentacionRequest req, CancellationToken ct)
        => Ok(await _svc.CrearPlanAsync(req, ct));

    [HttpGet("planes/{id:int}")]
    public async Task<ActionResult<PlanAlimentacionResponse>> ObtenerPlan(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerPlanAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet("planes")]
    public async Task<ActionResult<IReadOnlyList<PlanAlimentacionResponse>>> ListarPlanes(CancellationToken ct)
        => Ok(await _svc.ListarPlanesAsync(ct));

    [HttpGet("planes/finca/{idFinca:int}")]
    public async Task<ActionResult<IReadOnlyList<PlanAlimentacionResponse>>> ListarPorFinca(int idFinca, CancellationToken ct)
        => Ok(await _svc.ListarPorFincaAsync(idFinca, ct));

    [HttpPatch("planes/{id:int}/desactivar")]
    public async Task<IActionResult> DesactivarPlan(int id, CancellationToken ct)
    {
        await _svc.DesactivarPlanAsync(id, ct);
        return NoContent();
    }

    [HttpPost("detalles")]
    public async Task<ActionResult<DetallePlanAlimentacionResponse>> AgregarDetalle(
        [FromBody] AgregarDetallePlanRequest req, CancellationToken ct)
        => Ok(await _svc.AgregarDetalleAsync(req, ct));

    [HttpGet("planes/{idPlan:int}/detalles")]
    public async Task<ActionResult<IReadOnlyList<DetallePlanAlimentacionResponse>>> ListarDetalles(int idPlan, CancellationToken ct)
        => Ok(await _svc.ListarDetallesAsync(idPlan, ct));
}
