namespace BrahmanGan.Application.DTOs;

public record GestacionResponse(int Id, int IdAnimal, int? IdServicio, DateOnly FechaInicio,
    DateOnly? FechaPartoEstimado, DateOnly? FechaPartoReal, string Estado);
