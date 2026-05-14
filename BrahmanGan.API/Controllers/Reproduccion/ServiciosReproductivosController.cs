using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Reproduccion;

[ApiController]
[Route("api/servicios-reproductivos")]
public class ServiciosReproductivosController : ControllerBase
{
    private readonly IServicioReproductivoService _svc;
    public ServiciosReproductivosController(IServicioReproductivoService svc) => _svc = svc;

    [HttpPost("monta")]
    public async Task<ActionResult<ServicioResponse>> RegistrarMonta([FromBody] RegistrarMontaRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarMontaAsync(req, ct));

    [HttpPost("ia")]
    public async Task<ActionResult<ServicioResponse>> RegistrarIa([FromBody] RegistrarIaRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarIaAsync(req, ct));

    [HttpPut("{id:int}/confirmar")]
    public async Task<IActionResult> Confirmar(int id, [FromBody] ConfirmarServicioRequest req, CancellationToken ct)
    {
        await _svc.ConfirmarAsync(id, req, ct);
        return NoContent();
    }

    [HttpGet("hembra/{idHembra:int}")]
    public async Task<ActionResult<IReadOnlyList<ServicioResponse>>> ListarPorHembra(int idHembra, CancellationToken ct)
        => Ok(await _svc.ListarPorHembraAsync(idHembra, ct));
}
