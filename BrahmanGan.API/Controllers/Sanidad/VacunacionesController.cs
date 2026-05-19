using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Sanidad;

[ApiController]
[Route("api/[controller]")]
public class VacunacionesController : ControllerBase
{
    private readonly IVacunacionService _svc;
    public VacunacionesController(IVacunacionService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<VacunacionResponse>> Aplicar([FromBody] AplicarVacunaRequest req, CancellationToken ct)
        => Ok(await _svc.AplicarAsync(req, ct));

    [HttpGet("animal/{idAnimal:int}")]
    public async Task<ActionResult<IReadOnlyList<VacunacionResponse>>> ListarPorAnimal(int idAnimal, CancellationToken ct)
        => Ok(await _svc.ListarPorAnimalAsync(idAnimal, ct));

    [HttpGet("alertas")]
    public async Task<ActionResult<IReadOnlyList<VacunacionResponse>>> Alertas([FromQuery] int dias = 7, CancellationToken ct = default)
        => Ok(await _svc.ListarAlertasAsync(dias, ct));

    [HttpPost("desparasitacion")]
    public async Task<ActionResult<DesparasitacionResponse>> AplicarDesparasitacion([FromBody] AplicarDesparasitacionRequest req, CancellationToken ct)
        => Ok(await _svc.AplicarDesparasitacionAsync(req, ct));

    [HttpGet("desparasitacion/animal/{idAnimal:int}")]
    public async Task<ActionResult<IReadOnlyList<DesparasitacionResponse>>> ListarDesparasitacion(int idAnimal, CancellationToken ct)
        => Ok(await _svc.ListarDesparasitacionesPorAnimalAsync(idAnimal, ct));
}
