using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers;

/// <summary>
/// Administración de roles y permisos del sistema.
/// Requiere rol Administrador.
/// </summary>
[ApiController]
[Route("api/roles")]
[Authorize(Policy = "Administrador")]
[Produces("application/json")]
public sealed class RolesController : ControllerBase
{
    private readonly IRolServicio _roles;
    public RolesController(IRolServicio roles) => _roles = roles;

    /// <summary>Lista todos los roles con sus permisos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RolResponse>), 200)]
    public async Task<ActionResult<IEnumerable<RolResponse>>> Listar(CancellationToken ct) =>
        Ok(await _roles.ListarRolesAsync(ct));

    /// <summary>Obtiene un rol con sus permisos.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RolResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<RolResponse>> Obtener(int id, CancellationToken ct) =>
        Ok(await _roles.ObtenerRolAsync(id, ct));

    /// <summary>Crea un nuevo rol.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RolResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<RolResponse>> Crear(
        [FromBody] CrearRolRequest request,
        CancellationToken ct)
    {
        var rol = await _roles.CrearRolAsync(request, ct);
        return CreatedAtAction(nameof(Obtener), new { id = rol.Id }, rol);
    }

    /// <summary>Asigna un permiso a un rol.</summary>
    [HttpPost("permisos")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AsignarPermiso(
        [FromBody] AsignarPermisoRolRequest request,
        CancellationToken ct)
    {
        await _roles.AsignarPermisoAsync(request, ct);
        return NoContent();
    }

    /// <summary>Revoca un permiso de un rol.</summary>
    [HttpDelete("permisos")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RevocarPermiso(
        [FromBody] AsignarPermisoRolRequest request,
        CancellationToken ct)
    {
        await _roles.RevocarPermisoAsync(request, ct);
        return NoContent();
    }
}
