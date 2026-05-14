using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Leche;

[ApiController]
[Route("api/ventas-leche")]
public class VentasLecheController : ControllerBase
{
    private readonly IVentaLecheService _svc;
    public VentasLecheController(IVentaLecheService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<VentaLecheResponse>> Registrar([FromBody] RegistrarVentaLecheRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarAsync(req, ct));
}
