using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Leche;

[ApiController]
[Route("api/produccion-leche")]
public class ProduccionLecheController : ControllerBase
{
    private readonly IProduccionLecheService _svc;
    public ProduccionLecheController(IProduccionLecheService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<ProduccionLecheResponse>> Registrar([FromBody] RegistrarProduccionLecheRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarAsync(req, ct));
}
