namespace BrahmanGan.Application.DTOs;

// ===== Fase 8: Nómina =====
public record ContratarTrabajadorRequest(
    string Cedula, string Nombres, string Apellidos, DateOnly FechaIngreso,
    string? Cargo = null, decimal? SalarioBase = null, string? TipoContrato = null);

public record TrabajadorResponse(
    int Id, string Cedula, string Nombres, string Apellidos, string? Cargo,
    DateOnly FechaIngreso, DateOnly? FechaRetiro, decimal? SalarioBase,
    string? TipoContrato, bool Activo);

public record RegistrarPagoJornalRequest(
    int IdTrabajador, DateOnly Fecha, decimal ValorJornal,
    decimal? HorasTrabajadas = null, string? Concepto = null, int? IdCentro = null);

public record PagoJornalResponse(
    int Id, int IdTrabajador, DateOnly Fecha, decimal? HorasTrabajadas,
    decimal ValorJornal, string? Concepto, int? IdCentro);
