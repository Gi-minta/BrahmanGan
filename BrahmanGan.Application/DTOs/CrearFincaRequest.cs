namespace BrahmanGan.Application.DTOs;

// ===== Fase 2: Finca =====
public record CrearFincaRequest(string Nombre, int? IdMunicipio = null, string? NIT = null,
    string? Propietario = null, string? Direccion = null, string? Telefono = null,
    string? Email = null, decimal? AreaHectareas = null);
