namespace BrahmanGan.Application.DTOs;

// ===== Fase 7: Trazabilidad =====
public record EmitirRegistroICARequest(int IdAnimal, string TipoDocumento, string NumeroDocumento,
    DateOnly FechaExpedicion, DateOnly? FechaVencimiento = null,
    string? EntidadEmisora = null, int? IdMunicipio = null,
    string? UrlDocumento = null, string? Observaciones = null);
public record RegistroICAResponse(int Id, int IdAnimal, string TipoDocumento, string NumeroDocumento,
    DateOnly FechaExpedicion, DateOnly? FechaVencimiento, string? EntidadEmisora,
    int? IdMunicipio, string Estado, string? UrlDocumento, string? Observaciones);
