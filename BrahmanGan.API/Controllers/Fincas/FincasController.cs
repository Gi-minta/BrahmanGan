using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Fincas;

[ApiController]
[Route("api/[controller]")]
public class FincasController : ControllerBase
{
    private readonly IFincaService _svc;
    public FincasController(IFincaService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<FincaResponse>> Crear([FromBody] CrearFincaRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FincaResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet] public async Task<ActionResult<IReadOnlyList<FincaResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));
}
