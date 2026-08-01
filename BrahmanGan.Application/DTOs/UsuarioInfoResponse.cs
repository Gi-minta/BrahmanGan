namespace BrahmanGan.Application.DTOs;

/// <summary>Información básica del usuario autenticado.</summary>
public record UsuarioInfoResponse(
    int Id,
    string Email,
    string NombreCompleto,
    string[] Roles,
    string[] Permisos,
    /// <summary>
    /// La contraseña actual la fijó un administrador y es temporal: el cliente debe
    /// obligar a cambiarla antes de dejar trabajar.
    /// </summary>
    bool DebeCambiarPassword = false);
