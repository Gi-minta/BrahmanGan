using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Comercial;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _svc;
    public ClientesController(IClienteService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<ClienteResponse>> Crear([FromBody] CrearClienteRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteResponse>> Obtener(int id, CancellationToken ct)
    {
        var r = await _svc.ObtenerAsync(id, ct);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpGet] public async Task<ActionResult<IReadOnlyList<ClienteResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));
}
