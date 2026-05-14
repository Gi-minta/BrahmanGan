using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Animales;

[ApiController]
[Route("api/[controller]")]
public class RazasController : ControllerBase
{
    private readonly IRazaService _svc;
    public RazasController(IRazaService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<RazaResponse>> Crear([FromBody] CrearRazaRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet] public async Task<ActionResult<IReadOnlyList<RazaResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));
}
