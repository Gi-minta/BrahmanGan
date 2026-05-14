namespace BrahmanGan.Application.DTOs;

public record CrearContratoRequest(int IdCliente, string Tipo, DateOnly FechaInicio,
    DateOnly? FechaFin = null, decimal? PrecioAcordado = null, string? UnidadPrecio = null,
    decimal? VolumenEstimado = null, string? Condiciones = null);
