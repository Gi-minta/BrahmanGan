using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Equipos;

[ApiController]
[Route("api/maquinaria")]
public class MaquinariaController : ControllerBase
{
    private readonly IMaquinariaService _svc;
    public MaquinariaController(IMaquinariaService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<MaquinariaResponse>> Crear([FromBody] CrearMaquinariaRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MaquinariaResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaquinariaResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));

    [HttpPost("mantenimiento")]
    public async Task<ActionResult<MantenimientoEquipoResponse>> RegistrarMantenimiento(
        [FromBody] RegistrarMantenimientoRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarMantenimientoAsync(req, ct));

    [HttpGet("{id:int}/mantenimientos")]
    public async Task<ActionResult<IReadOnlyList<MantenimientoEquipoResponse>>> ListarMantenimientos(int id, CancellationToken ct)
        => Ok(await _svc.ListarMantenimientosAsync(id, ct));
}
