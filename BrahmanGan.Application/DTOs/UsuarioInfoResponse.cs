namespace BrahmanGan.Application.DTOs;

/// <summary>Información básica del usuario autenticado.</summary>
public record UsuarioInfoResponse(
    int Id,
    string Email,
    string NombreCompleto,
    string[] Roles,
    string[] Permisos);
