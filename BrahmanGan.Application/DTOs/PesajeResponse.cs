namespace BrahmanGan.Application.DTOs;

public record PesajeResponse(int Id, int IdAnimal, DateOnly Fecha, decimal PesoKg, decimal? CondicionCorporal);
