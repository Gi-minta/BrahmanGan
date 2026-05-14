namespace BrahmanGan.Application.DTOs;

/// <summary>Respuesta de un permiso atómico.</summary>
public record PermisoResponse(
    int Id,
    string Modulo,
    string Accion,
    string Clave,
    string Descripcion);
