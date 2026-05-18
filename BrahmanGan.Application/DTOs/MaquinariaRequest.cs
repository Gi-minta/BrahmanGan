namespace BrahmanGan.Application.DTOs;

// ===== Fase 7: Equipos =====
public record CrearMaquinariaRequest(int IdCentro, string Codigo, string Nombre,
    string? Marca = null, string? Modelo = null, int? Anio = null,
    string? NumeroSerie = null, DateOnly? FechaCompra = null, decimal? ValorCompra = null);
public record MaquinariaResponse(int Id, int IdCentro, string Codigo, string Nombre,
    string? Marca, string? Modelo, int? Anio, string? NumeroSerie,
    DateOnly? FechaCompra, decimal? ValorCompra, decimal HorasUso, string Estado);

public record RegistrarMantenimientoRequest(int IdMaquinaria, DateOnly Fecha,
    string TipoMantenimiento, string Descripcion,
    decimal CostoManoObra = 0, decimal CostoRepuestos = 0,
    string? Tecnico = null, string? Proveedor = null,
    DateOnly? ProximoMantenimiento = null, decimal? HorasAlMomento = null);
public record MantenimientoEquipoResponse(int Id, int IdMaquinaria, DateOnly Fecha,
    string TipoMantenimiento, string Descripcion, string? Tecnico, string? Proveedor,
    decimal CostoManoObra, decimal CostoRepuestos, decimal CostoTotal,
    DateOnly? ProximoMantenimiento, decimal? HorasAlMomento);
