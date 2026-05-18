namespace BrahmanGan.Application.DTOs;

// ===== Fase 7: Almacén =====
public record CrearInsumoRequest(string Codigo, string Nombre, string? Tipo = null,
    string? UnidadMedida = null, decimal? PrecioUnitario = null,
    decimal StockMinimo = 0, decimal StockInicial = 0);
public record InsumoResponse(int Id, string Codigo, string Nombre, string? Tipo,
    string? UnidadMedida, decimal? PrecioUnitario, decimal StockMinimo, decimal StockActual, bool Activo);

public record RegistrarMovimientoKardexRequest(int IdInsumo, DateOnly Fecha,
    string TipoMovimiento, decimal Cantidad,
    decimal? CostoUnitario = null, string? Concepto = null, string? Referencia = null);
public record KardexInsumoResponse(int Id, int IdInsumo, DateOnly Fecha, string TipoMovimiento,
    decimal Cantidad, decimal? CostoUnitario, string? Concepto, string? Referencia,
    decimal SaldoAnterior, decimal SaldoNuevo);
