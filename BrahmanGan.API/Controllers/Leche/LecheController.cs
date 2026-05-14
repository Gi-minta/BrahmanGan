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
}
