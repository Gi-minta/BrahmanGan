namespace BrahmanGan.Application.DTOs;

public record VacunacionResponse(int Id, int IdAnimal, int IdMedicamento, DateOnly Fecha,
    decimal? Dosis, string? Lote, DateOnly? ProximaFecha);
