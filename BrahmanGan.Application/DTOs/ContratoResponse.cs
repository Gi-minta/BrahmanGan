namespace BrahmanGan.Application.DTOs;

public record ContratoResponse(int Id, int IdCliente, string Tipo, DateOnly FechaInicio,
    DateOnly? FechaFin, decimal? PrecioAcordado, string Estado);
