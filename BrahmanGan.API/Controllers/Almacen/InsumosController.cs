using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Almacen;

[ApiController]
[Route("api/insumos")]
public class InsumosController : ControllerBase
{
    private readonly IInsumoService _svc;
    public InsumosController(IInsumoService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<InsumoResponse>> Crear([FromBody] CrearInsumoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InsumoResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InsumoResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));

    [HttpGet("bajo-minimo")]
    public async Task<ActionResult<IReadOnlyList<InsumoResponse>>> BajoMinimo(CancellationToken ct)
        => Ok(await _svc.ListarBajoMinimoAsync(ct));

    [HttpPost("movimiento")]
    public async Task<ActionResult<KardexInsumoResponse>> RegistrarMovimiento(
        [FromBody] RegistrarMovimientoKardexRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarMovimientoAsync(req, ct));

    [HttpGet("{id:int}/kardex")]
    public async Task<ActionResult<IReadOnlyList<KardexInsumoResponse>>> Kardex(int id, CancellationToken ct)
        => Ok(await _svc.ListarKardexAsync(id, ct));
}
