namespace BrahmanGan.Application.DTOs;

// ===== Banco de Semen =====
public record CrearSemenRequest(
    string Codigo, string NombreToro,
    int? IdRaza = null, string? Casa = null, int StockInicial = 0);

public record SemenResponse(
    int Id, string Codigo, string NombreToro,
    int? IdRaza, string? Casa, int StockDosis, bool Activo);

public record AjustarStockSemenRequest(int IdSemen, int Dosis, string Operacion);

public record NacimientoResponse(
    int Id, int IdGestacion, int? IdAnimalCria, DateOnly Fecha,
    string? Sexo, decimal? PesoNacimiento, string? Condicion, string? Observaciones);
