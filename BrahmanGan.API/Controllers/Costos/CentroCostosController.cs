using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Costos;

[ApiController]
[Route("api/centros-costo")]
public class CentroCostosController : ControllerBase
{
    private readonly ICentroCostoService _svc;
    public CentroCostosController(ICentroCostoService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<CentroCostoResponse>> Crear([FromBody] CrearCentroCostoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CentroCostoResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));
}
