namespace BrahmanGan.Application.DTOs;

public record ProduccionLecheResponse(int Id, int IdFinca, DateOnly Fecha,
    decimal TotalLitros, decimal? Vendidos, decimal? Autoconsumo, decimal? Merma);
