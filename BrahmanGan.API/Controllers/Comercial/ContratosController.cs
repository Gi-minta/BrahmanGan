using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Comercial;

[ApiController]
[Route("api/[controller]")]
public class ContratosController : ControllerBase
{
    private readonly IContratoService _svc;
    public ContratosController(IContratoService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<ContratoResponse>> Crear([FromBody] CrearContratoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("cliente/{idCliente:int}")]
    public async Task<ActionResult<IReadOnlyList<ContratoResponse>>> ListarPorCliente(int idCliente, CancellationToken ct)
        => Ok(await _svc.ListarPorClienteAsync(idCliente, ct));
}
