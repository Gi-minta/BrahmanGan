using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers.Animales;

[ApiController]
[Route("api/[controller]")]
public class PesajesController : ControllerBase
{
    private readonly IPesajeService _svc;
    public PesajesController(IPesajeService svc) => _svc = svc;

    [HttpPost] public async Task<ActionResult<PesajeResponse>> Registrar([FromBody] RegistrarPesajeRequest req, CancellationToken ct)
        => Ok(await _svc.RegistrarAsync(req, ct));

    [HttpGet("animal/{idAnimal:int}")]
    public async Task<ActionResult<IReadOnlyList<PesajeResponse>>> ListarPorAnimal(int idAnimal, CancellationToken ct)
        => Ok(await _svc.ListarPorAnimalAsync(idAnimal, ct));
}
