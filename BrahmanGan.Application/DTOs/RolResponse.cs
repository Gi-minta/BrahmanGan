namespace BrahmanGan.Application.DTOs;

/// <summary>Respuesta de un rol con sus permisos.</summary>
public record RolResponse(
    int Id,
    string Nombre,
    string Descripcion,
    bool EsSistema,
    bool Activo,
    PermisoResponse[] Permisos);
