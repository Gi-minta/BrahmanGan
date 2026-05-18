namespace BrahmanGan.Application.DTOs;

// ===== Desparasitación =====
public record AplicarDesparasitacionRequest(
    int IdAnimal, int IdMedicamento, DateOnly Fecha,
    decimal? Dosis = null, string? TipoParasito = null, DateOnly? ProximaFecha = null);

public record DesparasitacionResponse(
    int Id, int IdAnimal, int IdMedicamento, DateOnly Fecha,
    decimal? Dosis, string? TipoParasito, DateOnly? ProximaFecha);
