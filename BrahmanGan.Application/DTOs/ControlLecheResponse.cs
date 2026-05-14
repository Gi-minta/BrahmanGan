namespace BrahmanGan.Application.DTOs;

public record ControlLecheResponse(int Id, int IdAnimal, DateOnly Fecha,
    decimal? Maniana, decimal? Tarde, decimal? Noche, decimal Total);
