namespace BrahmanGan.Application.DTOs;

public record RegistrarPesajeRequest(int IdAnimal, DateOnly Fecha, decimal PesoKg,
    decimal? CondicionCorporal = null, string? MetodoPesaje = null, string? Responsable = null);
