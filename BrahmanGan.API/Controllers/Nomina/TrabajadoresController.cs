using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Nomina;

[ApiController]
[Route("api/trabajadores")]
public class TrabajadoresController : ControllerBase
{
    private readonly ITrabajadorService _svc;
    public TrabajadoresController(ITrabajadorService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<TrabajadorResponse>> Contratar([FromBody] ContratarTrabajadorRequest req, CancellationToken ct)
        => Ok(await _svc.ContratarAsync(req, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrabajadorResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TrabajadorResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));

    [HttpPost("pagos")]
    public async Task<ActionResult<PagoJornalResponse>> RegistrarPago([FromBody] RegistrarPagoJornalRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarPagoAsync(req, ct));

    [HttpGet("{id:int}/pagos")]
    public async Task<ActionResult<IReadOnlyList<PagoJornalResponse>>> ListarPagos(int id, CancellationToken ct)
        => Ok(await _svc.ListarPagosAsync(id, ct));
}
