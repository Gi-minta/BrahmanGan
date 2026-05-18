namespace BrahmanGan.Application.DTOs;

// ===== Fase 8: Sostenibilidad =====
public record RegistrarCapturaCarbonoRequest(
    int IdFinca, int Anio, int Mes,
    decimal? EmisionesGanadoTCO2 = null, decimal? CapturaForestal = null,
    string? Certificacion = null, string? Observaciones = null);

public record CapturaCarbonoResponse(
    int Id, int IdFinca, int Anio, int Mes,
    decimal? EmisionesGanadoTCO2, decimal? CapturaForestal, decimal HuellaNeta,
    string? Certificacion, string? Observaciones);

public record RegistrarConsumoAguaRequest(
    int IdFinca, DateOnly Fecha, decimal VolumenM3,
    int? IdPotrero = null, string? FuenteAgua = null,
    int? NumAnimales = null, string? Observaciones = null);

public record ConsumoAguaResponse(
    int Id, int IdFinca, int? IdPotrero, DateOnly Fecha, string? FuenteAgua,
    decimal VolumenM3, int? NumAnimales, decimal? LitrosAnimalDia, string? Observaciones);

public record RegistrarEventoMedioambientalRequest(
    int IdFinca, DateOnly Fecha, string TipoEvento,
    string? Descripcion = null, string? Intensidad = null,
    decimal? PrecipitacionMM = null, decimal? TempMaxC = null, decimal? TempMinC = null,
    string? ImpactoEstimado = null);

public record EventoMedioambientalResponse(
    int Id, int IdFinca, DateOnly Fecha, string TipoEvento,
    string? Descripcion, string? Intensidad,
    decimal? PrecipitacionMM, decimal? TempMaxC, decimal? TempMinC, string? ImpactoEstimado);
