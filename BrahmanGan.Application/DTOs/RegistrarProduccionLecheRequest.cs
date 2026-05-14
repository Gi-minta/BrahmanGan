namespace BrahmanGan.Application.DTOs;

public record RegistrarProduccionLecheRequest(int IdFinca, DateOnly Fecha, decimal TotalLitros,
    decimal? Vendidos = null, decimal? Autoconsumo = null, decimal? Merma = null);
