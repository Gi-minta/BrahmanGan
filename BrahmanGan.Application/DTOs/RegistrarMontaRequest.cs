namespace BrahmanGan.Application.DTOs;

// ===== Fase 3: Reproducción =====
public record RegistrarMontaRequest(int IdHembra, int IdToro, DateOnly Fecha, string? Responsable = null);
