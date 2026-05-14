namespace BrahmanGan.Application.DTOs;

// ===== Fase 5: Leche =====
public record RegistrarControlLecheRequest(int IdAnimal, DateOnly Fecha,
    decimal? Maniana = null, decimal? Tarde = null, decimal? Noche = null, string? Ordeno = null);
