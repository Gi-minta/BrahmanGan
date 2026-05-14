using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.API.Controllers;

/// <summary>
/// Administración de usuarios del sistema.
/// Requiere rol Administrador.
/// </summary>
[ApiController]
[Route("api/usuarios")]
[Authorize(Policy = "Administrador")]
[Produces("application/json")]
public sealed class UsuariosAdminController : ControllerBase
{
    private readonly IUsuarioAdminServicio _admin;
    public UsuariosAdminController(IUsuarioAdminServicio admin) => _admin = admin;

    /// <summary>Lista todos los usuarios.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), 200)]
    public async Task<ActionResult<IEnumerable<UsuarioResponse>>> Listar(CancellationToken ct) =>
        Ok(await _admin.ListarUsuariosAsync(ct));

    /// <summary>Obtiene un usuario con sus roles.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<UsuarioResponse>> Obtener(int id, CancellationToken ct) =>
        Ok(await _admin.ObtenerUsuarioAsync(id, ct));

    /// <summary>Asigna un rol a un usuario.</summary>
    [HttpPost("roles")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> AsignarRol(
        [FromBody] AsignarRolUsuarioRequest request,
        CancellationToken ct)
    {
        await _admin.AsignarRolAsync(request, ct);
        return NoContent();
    }

    /// <summary>Revoca un rol a un usuario.</summary>
    [HttpDelete("roles")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> RevocarRol(
        [FromBody] AsignarRolUsuarioRequest request,
        CancellationToken ct)
    {
        await _admin.RevocarRolAsync(request, ct);
        return NoContent();
    }

    /// <summary>Desactiva un usuario.</summary>
    [HttpPatch("{id:int}/desactivar")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Desactivar(int id, CancellationToken ct)
    {
        await _admin.DesactivarUsuarioAsync(id, ct);
        return NoContent();
    }

    /// <summary>Activa un usuario.</summary>
    [HttpPatch("{id:int}/activar")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Activar(int id, CancellationToken ct)
    {
        await _admin.ActivarUsuarioAsync(id, ct);
        return NoContent();
    }
}
