namespace BrahmanGan.Application.DTOs;

// ===== Fase 7: Costos =====
public record CrearCentroCostoRequest(string Codigo, string Nombre, int? IdFinca = null);
public record CentroCostoResponse(int Id, string Codigo, string Nombre, int? IdFinca, bool Activo);

public record CrearGastoGeneralRequest(DateOnly Fecha, string Concepto, decimal Valor,
    int? IdCentro = null, string? Proveedor = null, string? Comprobante = null);
public record GastoGeneralResponse(int Id, DateOnly Fecha, int? IdCentro, string Concepto,
    decimal Valor, string? Proveedor, string? Comprobante);

public record CrearIngresoRequest(DateOnly Fecha, int IdCentro, string TipoIngreso, decimal Valor,
    string? Concepto = null, string? Comprobante = null);
public record IngresoResponse(int Id, DateOnly Fecha, int IdCentro, string TipoIngreso,
    decimal Valor, string? Concepto, string? Comprobante);
