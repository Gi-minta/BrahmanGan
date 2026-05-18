namespace BrahmanGan.Application.DTOs;

// ===== Controles Preventivos =====
public record CrearControlPreventivoRequest(
    string Nombre, string? Periodicidad = null, string? Descripcion = null);

public record ControlPreventivoResponse(
    int Id, string Nombre, string? Periodicidad, string? Descripcion);

public record AplicarControlPreventivoRequest(
    int IdAnimal, int IdControl, DateOnly Fecha,
    int? IdMedicamento = null, decimal? Dosis = null,
    string? Responsable = null, DateOnly? ProximaFecha = null);

public record HistorialPreventivoResponse(
    int Id, int IdAnimal, int IdControl, DateOnly Fecha,
    int? IdMedicamento, decimal? Dosis, string? Responsable, DateOnly? ProximaFecha);
