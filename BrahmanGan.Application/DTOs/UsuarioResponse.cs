namespace BrahmanGan.Application.DTOs;

/// <summary>Respuesta de un usuario (vista admin).</summary>
public record UsuarioResponse(
    int Id,
    string Email,
    string NombreCompleto,
    string Proveedor,
    bool Activo,
    DateTime FechaCreacion,
    DateTime? UltimoAcceso,
    string[] Roles);
