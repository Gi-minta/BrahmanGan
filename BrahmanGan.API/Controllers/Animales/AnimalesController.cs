using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Animales;

[ApiController]
[Route("api/[controller]")]
public class AnimalesController : ControllerBase
{
    private readonly IAnimalService _svc;
    public AnimalesController(IAnimalService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<AnimalResponse>> Registrar([FromBody] CrearAnimalRequest req, CancellationToken ct)
    {
        var r = await _svc.RegistrarAsync(req, ct);
        return CreatedAtAction(nameof(Obtener), new { id = r.Id }, r);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnimalResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet("activos")]
    public async Task<ActionResult<IReadOnlyList<AnimalResponse>>> ListarActivos(CancellationToken ct)
        => Ok(await _svc.ListarActivosAsync(ct));

    [HttpGet("finca/{idFinca:int}")]
    public async Task<ActionResult<IReadOnlyList<AnimalResponse>>> ListarPorFinca(int idFinca, CancellationToken ct)
        => Ok(await _svc.ListarPorFincaAsync(idFinca, ct));

    [HttpPut("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoAnimalRequest req, CancellationToken ct)
    {
        await _svc.CambiarEstadoAsync(id, req, ct);
        return NoContent();
    }

    [HttpPut("{id:int}/trasladar")]
    public async Task<IActionResult> Trasladar(int id, [FromBody] TrasladarAnimalRequest req, CancellationToken ct)
    {
        await _svc.TrasladarAsync(id, req, ct);
        return NoContent();
    }

    [HttpGet("{id:int}/historial")]
    public async Task<ActionResult<IReadOnlyList<HistorialAnimalResponse>>> Historial(int id, CancellationToken ct)
        => Ok(await _svc.ListarHistorialAsync(id, ct));

    [HttpGet("{id:int}/movimientos")]
    public async Task<ActionResult<IReadOnlyList<MovimientoAnimalResponse>>> Movimientos(int id, CancellationToken ct)
        => Ok(await _svc.ListarMovimientosAsync(id, ct));

    [HttpPost("pedigri")]
    public async Task<ActionResult<PedigriResponse>> CrearPedigri([FromBody] CrearPedigriRequest req, CancellationToken ct)
        => Ok(await _svc.CrearPedigriAsync(req, ct));

    [HttpGet("{id:int}/pedigri")]
    public async Task<ActionResult<PedigriResponse>> ObtenerPedigri(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerPedigriAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }
}
