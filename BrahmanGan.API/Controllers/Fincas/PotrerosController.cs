using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Fincas;

[ApiController]
[Route("api/[controller]")]
public class PotrerosController : ControllerBase
{
    private readonly IPotreroService _svc;
    public PotrerosController(IPotreroService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<PotreroResponse>> Crear([FromBody] CrearPotreroRequest req, CancellationToken ct)
        => Ok(await _svc.CrearAsync(req, ct));

    [HttpGet("finca/{idFinca:int}")]
    public async Task<ActionResult<IReadOnlyList<PotreroResponse>>> ListarPorFinca(int idFinca, CancellationToken ct)
        => Ok(await _svc.ListarPorFincaAsync(idFinca, ct));

    // ── Grupos de Manejo ──────────────────────────────────────────
    [HttpPost("grupos")]
    public async Task<ActionResult<GrupoManejoResponse>> CrearGrupo([FromBody] CrearGrupoManejoRequest req, CancellationToken ct)
        => Ok(await _svc.CrearGrupoAsync(req, ct));

    [HttpGet("grupos")]
    public async Task<ActionResult<IReadOnlyList<GrupoManejoResponse>>> ListarGrupos(CancellationToken ct)
        => Ok(await _svc.ListarGruposAsync(ct));

    // ── Animales por Potrero ───────────────────────────────────────
    [HttpPost("asignaciones")]
    public async Task<ActionResult<AnimalPotreroResponse>> AsignarAnimal([FromBody] AsignarAnimalPotreroRequest req, CancellationToken ct)
        => Ok(await _svc.AsignarAnimalAsync(req, ct));

    [HttpPatch("asignaciones/{id:int}/cerrar")]
    public async Task<ActionResult<AnimalPotreroResponse>> CerrarAsignacion(int id, [FromBody] CerrarAnimalPotreroRequest req, CancellationToken ct)
        => Ok(await _svc.CerrarAsignacionAsync(id, req, ct));

    [HttpGet("{idPotrero:int}/animales")]
    public async Task<ActionResult<IReadOnlyList<AnimalPotreroResponse>>> ListarAnimales(int idPotrero, CancellationToken ct)
        => Ok(await _svc.ListarAnimalesPorPotreroAsync(idPotrero, ct));

    // ── Acumulación de Insumos ─────────────────────────────────────
    [HttpPost("acumulaciones")]
    public async Task<ActionResult<AcumulacionInsumoPotreroResponse>> RegistrarAcumulacion([FromBody] RegistrarAcumulacionInsumoRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarAcumulacionAsync(req, ct));

    [HttpGet("{idPotrero:int}/acumulaciones")]
    public async Task<ActionResult<IReadOnlyList<AcumulacionInsumoPotreroResponse>>> ListarAcumulaciones(int idPotrero, CancellationToken ct)
        => Ok(await _svc.ListarAcumulacionesPorPotreroAsync(idPotrero, ct));
}
