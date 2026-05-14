namespace BrahmanGan.Application.DTOs;

public record RegistrarVentaLecheRequest(DateOnly Fecha, int IdCliente, decimal Litros, decimal PrecioLitro,
    int? IdContrato = null, string? Factura = null);
