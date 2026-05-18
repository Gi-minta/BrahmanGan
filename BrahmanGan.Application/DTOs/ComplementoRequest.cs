namespace BrahmanGan.Application.DTOs;

// ===== Complementos de Tratamiento Curativo =====
public record RegistrarComplementoRequest(
    int IdTratamiento, DateOnly Fecha, string Descripcion,
    string? Tipo = null, decimal? Costo = null);

public record ComplementoResponse(
    int Id, int IdTratamiento, DateOnly Fecha,
    string Descripcion, string? Tipo, decimal? Costo);
