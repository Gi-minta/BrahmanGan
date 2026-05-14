using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Application.DTOs;
public record CotizacionResponse(int Id, int IdCliente, DateOnly Fecha, decimal PrecioOfertado, string Estado);
