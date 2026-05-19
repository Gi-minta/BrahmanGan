using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Sanidad;

[ApiController]
[Route("api/complementos")]
public class ComplementoController : ControllerBase
{
    private readonly IComplementoService _svc;
    public ComplementoController(IComplementoService svc) => _svc = svc;

    [HttpPost]
    public async Task<ActionResult<ComplementoResponse>> Registrar([FromBody] RegistrarComplementoRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarAsync(req, ct));

    [HttpGet("tratamiento/{idTratamiento:int}")]
    public async Task<ActionResult<IReadOnlyList<ComplementoResponse>>> ListarPorTratamiento(int idTratamiento, CancellationToken ct)
        => Ok(await _svc.ListarPorTratamientoAsync(idTratamiento, ct));
}
