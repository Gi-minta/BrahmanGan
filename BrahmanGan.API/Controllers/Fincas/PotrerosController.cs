using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Fincas;

[ApiController]
[Route("api/[controller]")]
public class PotrerosController : ControllerBase
{
    private readonly IPotreroService _svc;
    public PotrerosController(IPotreroService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<PotreroResponse>> Crear([FromBody] CrearPotreroRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("finca/{idFinca:int}")]
    public async Task<ActionResult<IReadOnlyList<PotreroResponse>>> ListarPorFinca(int idFinca, CancellationToken ct)
        => Ok(await _svc.ListarPorFincaAsync(idFinca, ct));
}
