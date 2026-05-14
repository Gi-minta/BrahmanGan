namespace BrahmanGan.Application.DTOs;

public record AnimalResponse(
    int Id, string Codigo, string? Nombre, int IdRaza, string Sexo, DateOnly? FechaNacimiento,
    int? IdMadre, int? IdPadre, int? IdOrigen, string Estado,
    decimal? PesoNacimiento, int IdFinca, DateOnly? FechaIngreso,
    string? Observaciones, DateTime FechaRegistro);
