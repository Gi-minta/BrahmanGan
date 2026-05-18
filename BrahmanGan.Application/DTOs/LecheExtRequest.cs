namespace BrahmanGan.Application.DTOs;

// ===== Parámetros de Lactancia =====
public record IniciarParametroLactanciaRequest(
    int IdAnimal, int NumeroParto, DateOnly FechaInicio);

public record CerrarParametroLactanciaRequest(
    DateOnly FechaFin, decimal? LitrosTotales = null);

public record ParametroLactanciaResponse(
    int Id, int IdAnimal, int NumeroParto,
    DateOnly FechaInicio, DateOnly? FechaFin, decimal? LitrosTotales);

// ===== Calidad de Leche =====
public record RegistrarCalidadLecheRequest(
    DateOnly Fecha, int? IdAnimal = null,
    int? CelSomaticas = null, decimal? GrasaPct = null,
    decimal? ProteinaPct = null, decimal? LactozaPct = null,
    decimal? UreaMgDL = null, string? Laboratorio = null,
    string? Resultado = null, string? Observaciones = null);

public record CalidadLecheResponse(
    int Id, int? IdAnimal, DateOnly Fecha,
    int? CelSomaticas, decimal? GrasaPct, decimal? ProteinaPct,
    decimal? LactozaPct, decimal? UreaMgDL,
    string? Laboratorio, string? Resultado, string? Observaciones);
