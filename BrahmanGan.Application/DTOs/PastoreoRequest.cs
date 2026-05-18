namespace BrahmanGan.Application.DTOs;

// ===== Pastoreo =====
public record CrearPlanPastoreoRequest(
    int IdPotrero, DateOnly FechaInicio,
    DateOnly? FechaFin = null, int? NumAnimales = null,
    decimal? CapacidadCarga = null, string? Observaciones = null);

public record PlanPastoreoResponse(
    int Id, int IdPotrero, DateOnly FechaInicio, DateOnly? FechaFin,
    int? NumAnimales, decimal? CapacidadCarga, string? Observaciones, bool Activo);
