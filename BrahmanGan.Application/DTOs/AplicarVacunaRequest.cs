namespace BrahmanGan.Application.DTOs;

public record AplicarVacunaRequest(int IdAnimal, int IdMedicamento, DateOnly Fecha,
    decimal? Dosis = null, string? Lote = null, string? Responsable = null, DateOnly? ProximaFecha = null);
