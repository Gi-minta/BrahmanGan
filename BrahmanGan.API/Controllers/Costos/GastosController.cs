using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Costos;

[ApiController]
[Route("api/gastos-generales")]
public class GastosController : ControllerBase
{
    private readonly IGastoGeneralService _svc;
    public GastosController(IGastoGeneralService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<GastoGeneralResponse>> Crear([FromBody] CrearGastoGeneralRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GastoGeneralResponse>>> ListarPorPeriodo(
        [FromQuery] DateOnly desde, [FromQuery] DateOnly hasta, CancellationToken ct)
        => Ok(await _svc.ListarPorPeriodoAsync(desde, hasta, ct));
}
