using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Sanidad;

[ApiController]
[Route("api/[controller]")]
public class MedicamentosController : ControllerBase
{
    private readonly IMedicamentoService _svc;
    public MedicamentosController(IMedicamentoService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<MedicamentoResponse>> Crear([FromBody] CrearMedicamentoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet] public async Task<ActionResult<IReadOnlyList<MedicamentoResponse>>> Listar(CancellationToken ct)
        => Ok(await _svc.ListarAsync(ct));
}
