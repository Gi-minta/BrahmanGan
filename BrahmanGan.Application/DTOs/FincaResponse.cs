namespace BrahmanGan.Application.DTOs;

public record FincaResponse(int Id, string Nombre, string? Nit, string? Propietario,
    int? IdMunicipio, decimal? AreaHectareas, bool Activa);
