namespace BrahmanGan.Application.DTOs;

public record FincaResponse(int Id, string Nombre, string? NIT, string? Propietario,
    int? IdMunicipio, decimal? AreaHectareas, bool Activa);
