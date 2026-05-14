namespace BrahmanGan.Application.DTOs;

// ===== Fase 6: Comercial =====
public record CrearClienteRequest(string Documento, string RazonSocial,
    string TipoDocumento = "NIT", string? Contacto = null, string? Telefono = null,
    string? Email = null, string? Direccion = null, int? IdMunicipio = null, string? TipoCliente = null);
