using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Trazabilidad;

[ApiController]
[Route("api/registros-ica")]
public class RegistroICAController : ControllerBase
{
    private readonly IRegistroICAService _svc;
    public RegistroICAController(IRegistroICAService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<RegistroICAResponse>> Emitir([FromBody] EmitirRegistroICARequest req, CancellationToken ct)
        => Ok(await _svc.EmitirAsync(req, ct));

    [HttpGet("animal/{idAnimal:int}")]
    public async Task<ActionResult<IReadOnlyList<RegistroICAResponse>>> ListarPorAnimal(int idAnimal, CancellationToken ct)
        => Ok(await _svc.ListarPorAnimalAsync(idAnimal, ct));

    [HttpGet("proximos-vencer")]
    public async Task<ActionResult<IReadOnlyList<RegistroICAResponse>>> ProximosVencer(
        [FromQuery] int dias = 30, CancellationToken ct = default)
        => Ok(await _svc.ListarProximosVencerAsync(dias, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancelar(int id, CancellationToken ct)
    {
        await _svc.CancelarAsync(id, ct);
        return NoContent();
    }
}
