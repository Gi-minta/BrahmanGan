namespace BrahmanGan.Application.DTOs;

public record CrearCotizacionRequest(int IdCliente, DateOnly Fecha, decimal PrecioOfertado,
    DateOnly? FechaVigencia = null, string? UnidadPrecio = null, string? Observaciones = null);
