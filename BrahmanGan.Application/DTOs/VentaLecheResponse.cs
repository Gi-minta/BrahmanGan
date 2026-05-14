namespace BrahmanGan.Application.DTOs;

public record VentaLecheResponse(int Id, DateOnly Fecha, int IdCliente, decimal Litros,
    decimal PrecioLitro, decimal Total, int? IdContrato);
