namespace BrahmanGan.Application.DTOs;

public record PotreroResponse(int Id, int IdFinca, string Codigo, string Nombre,
    decimal? AreaHectareas, string? TipoPasto, bool Activo);
