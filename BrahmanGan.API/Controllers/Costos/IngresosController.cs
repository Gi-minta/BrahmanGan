using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Costos;

[ApiController]
[Route("api/ingresos")]
public class IngresosController : ControllerBase
{
    private readonly IIngresoService _svc;
    public IngresosController(IIngresoService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<IngresoResponse>> Crear([FromBody] CrearIngresoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("centro/{idCentro:int}")]
    public async Task<ActionResult<IReadOnlyList<IngresoResponse>>> ListarPorCentro(int idCentro, CancellationToken ct)
        => Ok(await _svc.ListarPorCentroAsync(idCentro, ct));
}
