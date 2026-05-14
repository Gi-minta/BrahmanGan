using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Reproduccion;

[ApiController]
[Route("api/[controller]")]
public class GestacionesController : ControllerBase
{
    private readonly IGestacionService _svc;
    public GestacionesController(IGestacionService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<GestacionResponse>> Iniciar([FromBody] IniciarGestacionRequest req, CancellationToken ct)
        => Ok(await _svc.IniciarAsync(req, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GestacionResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpPut("{id:int}/parto")]
    public async Task<IActionResult> Parto(int id, [FromBody] RegistrarPartoRequest req, CancellationToken ct)
    { await _svc.RegistrarPartoAsync(id, req, ct); return NoContent(); }

    [HttpPut("{id:int}/aborto")]
    public async Task<IActionResult> Aborto(int id, [FromBody] RegistrarAbortoRequest req, CancellationToken ct)
    { await _svc.RegistrarAbortoAsync(id, req, ct); return NoContent(); }
}
