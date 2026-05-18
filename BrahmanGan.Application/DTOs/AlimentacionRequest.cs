namespace BrahmanGan.Application.DTOs;

// ===== Alimentación =====
public record CrearPlanAlimentacionRequest(
    int IdFinca, string Nombre, DateOnly FechaInicio,
    DateOnly? FechaFin = null, string? Observaciones = null);

public record PlanAlimentacionResponse(
    int Id, int IdFinca, string Nombre,
    DateOnly FechaInicio, DateOnly? FechaFin,
    string? Observaciones, bool Activo);

public record AgregarDetallePlanRequest(
    int IdPlan, string Alimento, decimal CantidadDiaria,
    string? UnidadMedida = null, int? IdInsumo = null, string? Observaciones = null);

public record DetallePlanAlimentacionResponse(
    int Id, int IdPlan, string Alimento, decimal CantidadDiaria,
    string? UnidadMedida, int? IdInsumo, string? Observaciones);
