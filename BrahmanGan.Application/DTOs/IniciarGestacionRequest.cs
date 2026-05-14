namespace BrahmanGan.Application.DTOs;

public record IniciarGestacionRequest(int IdAnimal, DateOnly FechaInicio, int? IdServicio = null, string? Observaciones = null);
