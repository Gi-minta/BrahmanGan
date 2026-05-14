namespace BrahmanGan.Application.DTOs;

public record CrearPotreroRequest(int IdFinca, string Codigo, string Nombre,
    decimal? AreaHectareas, string? TipoPasto);
