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

    [HttpPost("controles")]
    public async Task<ActionResult<ControlPreventivoResponse>> CrearControl([FromBody] CrearControlPreventivoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearControlAsync(req, ct));

    [HttpGet("controles")]
    public async Task<ActionResult<IReadOnlyList<ControlPreventivoResponse>>> ListarControles(CancellationToken ct)
        => Ok(await _svc.ListarControlesAsync(ct));

    [HttpPost("controles/aplicar")]
    public async Task<ActionResult<HistorialPreventivoResponse>> AplicarControl([FromBody] AplicarControlPreventivoRequest req, CancellationToken ct)
        => Ok(await _svc.AplicarControlAsync(req, ct));

    [HttpGet("controles/historial/{idAnimal:int}")]
    public async Task<ActionResult<IReadOnlyList<HistorialPreventivoResponse>>> HistorialPreventivo(int idAnimal, CancellationToken ct)
        => Ok(await _svc.ListarHistorialPreventivoAsync(idAnimal, ct));
}
