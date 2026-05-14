namespace BrahmanGan.Application.DTOs;

public record ClienteResponse(int Id, string Documento, string TipoDocumento, string RazonSocial,
    string? Contacto, string? Telefono, string? Email, bool Activo);
