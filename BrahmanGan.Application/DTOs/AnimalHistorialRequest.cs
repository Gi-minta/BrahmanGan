namespace BrahmanGan.Application.DTOs;

// ===== Historial del Animal =====
public record HistorialAnimalResponse(
    int Id, int IdAnimal, string TipoEvento, DateOnly Fecha,
    string? Descripcion, decimal? Valor, string? UsuarioRegistro, DateTime FechaRegistro);

// ===== Movimientos del Animal =====
public record MovimientoAnimalResponse(
    int Id, int IdAnimal, string TipoMovimiento, DateOnly Fecha,
    decimal? Valor, int? IdCentro, decimal? PesoKg, string? Observaciones);
